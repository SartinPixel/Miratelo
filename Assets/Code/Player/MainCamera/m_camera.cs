using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Triheroes.Code
{
    public class m_camera : module
    {
        public static m_camera o;

        public Transform Coord;
        public Transform CameraPivot;
        public Camera Cam;
        public Animation Ani;

        [Depend]
        public m_camera_tps_smooth mcts;

        public override void Create()
        {
            o = this;
            FetchCameraData();
            mcts.Aquire(this);
        }

        void FetchCameraData()
        {
            Coord = character.transform;
            CameraBase A = character.GetComponent<CameraBase>();
            CameraPivot = A.CameraPivot;
            Cam = A.Cam;
            Ani = A.GetComponent<Animation>();
        }

        // Screen ray
        Ray ScreenRay;
        // public methods
        public Vector3 PointScreenCenter(Transform Exclude)
        {
            // Update ScreenRay
            ScreenRay.origin = Coord.position;
            ScreenRay.direction = Coord.forward;

            int d = Exclude.gameObject.layer;
            Exclude.gameObject.layer = 0;
            RaycastHit hit;

            bool HasHitSomething = Physics.Raycast(ScreenRay, out hit,
            256, Vecteur.SolidCharacter);

            Exclude.gameObject.layer = d;

            if (HasHitSomething)
                return hit.point;
            else
                return Coord.position + Coord.forward * 256;
        }
    }

    public abstract class m_camera_controller : core
    {
        public abstract void Main();
    }

    public class s_camera_controller : CoreSystem<m_camera_controller>
    {
        protected override void Main(m_camera_controller o)
        {
            o.Main();
        }
    }

    [RegisterAsBase]
    public class m_camera_tps_smooth : m_camera_controller
    {
        [Depend]
        public m_camera mc;

        public m_actor C;

        // controls
        public Vector3 rotY { get; private set; }
        public Vector3 offset { get; private set; }
        public float height { get; private set; }
        public float distance { get; private set; }

        pc_camera_tps_controller state;
        m_camera_tps_transition StateTransition;
        bool inTransition;

        public tps_camera tps;
        public tps_camera_target target;

        public override void Create()
        {
            StateTransition = character.ConnectNode(new m_camera_tps_transition());

            tps = character.ConnectNode(new tps_camera());
            target = character.ConnectNode(new tps_camera_target());
            state = tps;
        }

        void SetState(pc_camera_tps_controller newState)
        {
            if (newState != state)
            {
                state = newState;
                StateTransition.SetNext(newState);
                StateTransition.Default();
                inTransition = true;
            }
        }

        public sealed override void Main()
        {
            CameraController();
            UpdateState();
            SetTpsCamera();
        }

        void CameraController()
        {

            if (C.target != null)
            {
                SetState(target);
                return;
            }

            SetState(tps);

        }

        void UpdateState()
        {
            if (!inTransition)
            {
                state.Update();
                rotY = state.rotY;
                offset = state.offset;
                distance = state.distance;
                height = state.height;
            }
            else
            {
                StateTransition.Update();
                rotY = StateTransition.rotY;
                offset = StateTransition.offset;
                distance = StateTransition.distance;
                height = StateTransition.height;
            }
        }

        void SetTpsCamera()
        {
            float RayDistance = distance;

            if (Physics.Raycast(mc.Coord.position, mc.CameraPivot.TransformDirection(Vector3.back), out RaycastHit hit, distance, Vecteur.Solid))
                RayDistance = hit.distance - 0.25f;

            mc.Coord.position = C.md.position + offset + height * Vector3.up;
            mc.Coord.rotation = Quaternion.Euler(rotY);
            mc.CameraPivot.transform.localPosition = Vector3.back * RayDistance;
        }

        class m_camera_tps_transition : pc_camera_tps_controller
        {
            pc_camera_tps_controller next;

            // parameters
            public Vector3 inRotY;
            public Vector3 inOffset;
            public float inHeight;
            public float inDistance;

            pc_camera_tps_controller nextState;
            public void SetNext(pc_camera_tps_controller next)
            {
                nextState = next;
            }

            public override void Default()
            {
                t = 0;
                UpdateFromC();

                inRotY = c.rotY;
                inOffset = c.offset;
                inHeight = c.height;
                inDistance = c.distance;

                nextState.Default();
            }

            float t = 0;
            public override void Update()
            {
                nextState.Update();

                // TODO make independent of framerate
                // TODO use tween
                t = Mathf.Lerp(t, 1, .1f);

                rotY.y = Mathf.LerpAngle(inRotY.y, nextState.rotY.y, t);
                rotY.x = Mathf.LerpAngle(inRotY.x, nextState.rotY.x, t);
                height = Mathf.Lerp(inHeight, nextState.height, t);
                distance = Mathf.Lerp(inDistance, nextState.distance, t);
                offset = Vector3.Lerp(inOffset, nextState.offset, t);

                if (t >= .99f)
                    c.inTransition = false;

                return;
            }

        }
    }

    public abstract class pc_camera_tps_controller : node
    {
        [Depend]
        protected m_camera_tps_smooth c;
        // parameters
        public Vector3 rotY;
        public Vector3 offset;
        public float height;
        public float distance;

        protected void UpdateFromC()
        {
            rotY = c.rotY;
            offset = c.offset;
            height = c.height;
            distance = c.distance;
        }

        public virtual void Default()
        { }

        public virtual void Update()
        { }
    }
}
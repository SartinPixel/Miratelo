using System.Collections;
using System.Collections.Generic;
using Pixify;
using Pixify.Spirit;
using UnityEngine;

namespace Triheroes.Code
{
    public class ar_way_arround_target : reflexion
    {
        [Depend]
        t_move_arround_target tmat;

        [Depend]
        ac_way_arround_target awat;

        protected override void Step()
        {
            if (tmat.on && !awat.on)
            Stage.Start ( awat );
        }
    }

    public class ac_way_arround_target : action
    {
        float AngleAmount;
        float Distance;
        int WaypointsCount;
        float InitalRotY;

        [Depend]
        d_actor da;

        [Depend]
        t_move_arround_target tmat;

        [Depend]
        ar_move_way_point amwp;

        protected override void Start()
        {
            if ( !da.target ) return;
            AngleAmount = tmat.angleAmount;
            Distance = tmat.distance;

            InitalRotY = Vecteur.RotDirection ( da.target.dd.position, da.dd.position ).y;

            WaypointsCount =  1 + (int) AngleAmount / 10;
            var points = new Vector3 [ WaypointsCount ];
            
            amwp.SetWayPoints ( points );

            ModifyCircleWay ();
        }

        protected override void Step()
        {
            if ( !da.target || amwp.Count == 0 )
            {
                tmat.Finish ();
                SelfStop ();
                return;
            }

            ModifyCircleWay ();
        }

        
        void ModifyCircleWay ()
        {
            for (int i = 0; i < amwp.Count - 1; i++)
            {
                amwp.SetPoint (i, da.target.dd.position + Vecteur.LDir ( new Vector3(0,InitalRotY + (i + WaypointsCount - amwp.Count ) * 10,0), Vector3.forward * Distance ) );
            }

            if ( amwp.Count > 0 )
            amwp.SetPoint ( amwp.Count - 1, da.target.dd.position + Vecteur.LDir ( new Vector3(0,InitalRotY + AngleAmount,0), Vector3.forward * Distance ) );
        }
    }
}
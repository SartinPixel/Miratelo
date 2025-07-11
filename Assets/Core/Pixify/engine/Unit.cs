using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Pixify
{
    public class UnitPoolMaster : module
    {
        static UnitPoolMaster o;

        Dictionary<int, Unit.UnitPool> Pools;

        public override void Create()
        {
            o = this;
            Pools = new Dictionary<int, Unit.UnitPool>();

            UnitAuthor[] Authors = SubResources<UnitAuthor>.GetAll();

            for (int i = 0; i < Authors.Length; i++)
            {
                var u = new Unit.UnitPool(Authors[i]);
                Pools.Add(new SuperKey(Authors[i].name), u);
            }
        }

        public static void AddPool ( IUnitAuthor author, string name )
        {
            o.Pools.Add ( new SuperKey (name), new Unit.UnitPool (author) );
        }

        public static void GetUnit ( int name )
        {
            o.Pools[name].GetUnit();
        }
    }

    public sealed class Unit : atom
    {
        UnitPool Parent;
        Dictionary<Type, piece> pieces = new Dictionary<Type, piece>();
        List<piece> pieceList = new List<piece>();

        public void Start()
        {
            foreach (var p in pieceList)
                p.Aquire (this);
        }

        public void Free()
        {
            foreach (var p in pieceList)
                p.Free (this);
        }

        piece RequirePiece(Type pieceType)
        {
            if (!pieceType.IsSubclassOf(typeof(piece)))
                throw new InvalidOperationException("cannot depend on a non piece type");

            if (pieces.TryGetValue(pieceType, out piece p))
                return p;
            else
            {
                p = Activator.CreateInstance(pieceType) as piece;
                pieces.Add(p.GetType(), p);
                RegisterPiece(p);
                p.Create();

                return p;
            }
        }

        public T RequirePiece<T>() where T : piece, new()
        {
            if (pieces.TryGetValue(typeof(T), out piece p1))
                return p1 as T;
            else
            {
                var p = new T();
                pieces.Add(p.GetType(), p);
                RegisterPiece(p);
                p.Create();
                return p;
            }
        }

        public T GetPiece<T>() where T : piece
        {
            if (pieces.TryGetValue(typeof(T), out piece p))
                return p as T;
            else
                return null;
        }

        void RegisterPiece(piece p)
        {
            pieceList.Add(p);
            p.unit = this;

            Type current = p.GetType();
            while (current != typeof(atom))
            {
                var fis = current.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var fi in fis)
                {
                    if (fi.GetCustomAttribute<DependAttribute>() != null)
                        fi.SetValue(p, RequirePiece(fi.FieldType));
                }
                current = current.BaseType;
            }
        }

        public void Return_()
        {
            Parent.ReturnUnit(this);
        }

        public class UnitPool
        {
            Queue<Unit> queue;

            List<Unit> pendingUnit = new List<Unit>();
            int currentFrame;

            IUnitAuthor author;

            public UnitPool(IUnitAuthor author)
            {
                this.author = author;
                queue = new Queue<Unit>();
            }

            public void GetUnit()
            {
                CheckCapacity();

                var u = queue.Dequeue();
                u.Start();
            }

            void CheckCapacity()
            {
                if (queue.Count == 0)
                {
                    var u = author.Instance();
                    u.Parent = this;
                    queue.Enqueue(u);
                }
            }

            public void ReturnUnit(Unit u)
            {
                u.Free();
                pendingUnit.Add(u);
                currentFrame = Time.frameCount;

                // to make sure the unit is not used again in the same frame, they are moved to the pending list first then reused on a later frame
                if ( Time.frameCount != currentFrame && pendingUnit.Count > 0 )
                {
                    foreach (var p in pendingUnit)
                        queue.Enqueue(p);
                    pendingUnit.Clear();
                }
            }

        }
    }
}
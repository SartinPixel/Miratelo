using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
	public class Vecteur : MonoBehaviour
	{
		public static Vecteur o;

		// layer mask constant
		public static LayerMask TempLayer;
		public static LayerMask Solid;
		public static LayerMask Attack;
		public static LayerMask Character;
		public static LayerMask SolidCharacterAttack;
		public static LayerMask SolidCharacter;
		public static LayerMask Interact;

		// layer hardcoded constant
		public static readonly int ATTACK = 11;
		public static readonly float Drag = 1;

		// initialisation
		void Awake()
		{
			Cursor.lockState = CursorLockMode.Locked;
			o = this;

			TempLayer = LayerMask.GetMask("temp");
			Solid = LayerMask.GetMask("solid");
			Attack = LayerMask.GetMask("attack");
			Character = LayerMask.GetMask("character");
			SolidCharacter = LayerMask.GetMask("solid", "character");
			SolidCharacterAttack = LayerMask.GetMask("solid", "character", "attack");
			Interact = LayerMask.GetMask("interact");
		}

		/// <summary> get a vector rotated by quaternion rotation </summary>
		public static Vector3 LDir(Quaternion Rotation, Vector3 Direction) => Rotation * Direction;

		/// <summary> get a vector rotated by quaternion rotation </summary>
		public static Vector3 LDir(Vector3 rotation, Vector3 Direction) => LDir(Quaternion.Euler(rotation), Direction);

		/// <summary> get y angle when seeing from position to another position </summary>	
		public static float RotDirectionY(Vector3 from, Vector3 to) => Quaternion.LookRotation(to - from).eulerAngles.y;

		/// <summary> get euleur angle when seeing from position to another position </summary>	
		public static Vector3 RotDirection(Vector3 from, Vector3 to) => Quaternion.LookRotation(to - from).eulerAngles;

		public static Vector3 Forward(Quaternion rotation) => rotation * Vector3.forward;
		public static Quaternion RotDirectionQuaternion(Vector3 from, Vector3 to) => Quaternion.LookRotation(to - from);
	}

	public static class Vector3Extensions
	{
		public static Vector3 OnlyY(this Vector3 V) => new Vector3(0, V.y, 0);
		public static Vector3 OnlyZX(this Vector3 V) => new Vector3(V.x, 0, V.z);

		public static Vector3 Flat(this Vector3 V) => new Vector3(V.x, 0, V.z);
	}

	public static class QuaternionExtensions
	{
		public static Quaternion AppliedAfter(this Quaternion Q, Vector3 V) => Q * Quaternion.Euler(V);
	}

	public static class DictionaryExtensions
	{
		public static void AddOrChange<T, T1>(this Dictionary<T, T1> d, T key, T1 value)
		{
			if (d.ContainsKey(key))
				d[key] = value;
			else
				d.Add(key, value);
		}

		// will return an new if the given key does not exisit
		public static T1 ForceGet<T, T1>(this Dictionary<T, T1> d, T key) where T1 : new()
		{
			if (d.ContainsKey(key))
				return d[key];
			else
			{
				var a = new T1();
				d.Add(key, a);
				return a;
			}
		}
	}

	public static class CollisionExtensions
	{
		public static int id(this Collision c) => c.collider.gameObject.GetInstanceID();
	}

	public static class ColliderExtensions
	{
		public static int id(this Collider c) => c.gameObject.GetInstanceID();
	}
}
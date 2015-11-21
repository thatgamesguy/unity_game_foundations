using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	public class ExplosionEvent : GameEvent
	{
		private Vector3 position;
		public Vector3 Position { get { return position; } }
		
		private float radius;
		public float Radius {
			get {
				return radius;
			}
		}
		
		private float force;
		public float Force {
			get {
				return force;
			}
		}

		public ExplosionEvent (Vector3 position, float radius, float force)
		{
			this.position = position;
			this.radius = radius;
			this.force = force;
		}
		
		public bool InExplosionRadius (Vector3 ObjPosition)
		{
			return Vector3.Distance (Position, ObjPosition) <= radius;
		}


	}
}

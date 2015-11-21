using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	public class SphereController : ShapeController
	{
		void OnEnable ()
		{
			Events.instance.AddListener<SphereSelectedEvent> (OnSelected);
			Events.instance.AddListener<ExplosionEvent> (OnExplosion);
		}
		
		void OnDisable ()
		{
			Events.instance.RemoveListener<SphereSelectedEvent> (OnSelected);
			Events.instance.RemoveListener<ExplosionEvent> (OnExplosion);
		}
		
	}
}

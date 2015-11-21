using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	public class CubeController : ShapeController
	{	
		void OnEnable ()
		{
			Events.instance.AddListener<CubeSelectedEvent> (OnSelected);
			Events.instance.AddListener<ExplosionEvent> (OnExplosion);
		}
		
		void OnDisable ()
		{
			Events.instance.RemoveListener<CubeSelectedEvent> (OnSelected);
			Events.instance.RemoveListener<ExplosionEvent> (OnExplosion);
		}
	
	
		
	}
}

using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	public class MouseController : MonoBehaviour
	{
		public AudioClip ExplosionSound;
		
		private const float RaycastDistance = 400f;
	
		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
			
				if (Physics.Raycast (ray, out hit, RaycastDistance)) {
					if (hit.transform.CompareTag ("Cube")) {
						Events.instance.Raise (new CubeSelectedEvent ());
					} else if (hit.transform.CompareTag ("Sphere")) {
						Events.instance.Raise (new SphereSelectedEvent ());
					}
				}
			} else if (Input.GetMouseButtonDown (1)) {
				var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast (ray, out hit, RaycastDistance)) {
					if (hit.transform.CompareTag ("Cube") || hit.transform.CompareTag ("Sphere")) {
						Events.instance.Raise (new ExplosionEvent (hit.transform.position, 10f, 2000f));
						Events.instance.Raise (new AudioEvent2D (ExplosionSound));
					} 
				}
			}
		}
	}
}

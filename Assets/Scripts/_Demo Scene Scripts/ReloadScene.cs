using UnityEngine;
using System.Collections;

public class ReloadScene : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
	{
	
		if (Input.GetKeyUp (KeyCode.R)) {
			Application.LoadLevel (0);
		}
	
	}
}

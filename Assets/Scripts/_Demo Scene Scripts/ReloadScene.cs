using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.R)) {
			SceneManager.LoadScene (0);
		}
	}
}

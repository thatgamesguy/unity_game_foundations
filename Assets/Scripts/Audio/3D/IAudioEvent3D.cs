using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	public interface IAudioEvent3D
	{
		Vector3 Position { get; }
		AudioClip Audio { get; }
	}
}

using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	[RequireComponent (typeof(AudioSource))]
	public class AudioPlayer3D : MonoBehaviour
	{
		public int MaxPending = 30;
		
		private AudioSource source;
		private IAudioEvent3D[] pending;
		
		private int head;
		private int tail;
		
		void Awake ()
		{
			source = GetComponent<AudioSource> ();
			source.spatialBlend = 1f;
		}
		
		void OnEnable ()
		{
			head = tail = 0;
			
			pending = new IAudioEvent3D [MaxPending];
			
			Events.instance.AddListener<AudioEvent3D> (OnAudio);
		}
		
		void OnDisable ()
		{
			Events.instance.RemoveListener<AudioEvent3D> (OnAudio);
		}
		
		void Update ()
		{
			if (head == tail)
				return;
			
			Debug.Log ("Playing: " + pending [head].Audio.name + " at position: " + pending [head].Position);
			
			AudioSource.PlayClipAtPoint (pending [head].Audio, pending [head].Position);
			
			source.PlayOneShot (pending [head].Audio);
			
			head = (head + 1) % MaxPending;
		}
		
		void OnAudio (IAudioEvent3D e)
		{
			for (int i = head; i != tail; i = (i+1) % MaxPending) {
				Debug.Log (i + ": " + pending [i].Audio.name);
				if (pending [i].Audio.name.Equals (e.Audio.name)) {
					return;
				}
			}
			
			pending [tail] = e;
			tail = (tail + 1) % MaxPending;
		}
		
	}
}

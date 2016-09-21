using UnityEngine;
using System.Collections;


public class AudioEvent3D : GameEvent, IAudioEvent3D
{
    private Vector3 position;
    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    private AudioClip audio;
    public AudioClip Audio
    {
        get
        {
            return audio;
        }
    }

    public AudioEvent3D(AudioClip audio, Vector3 position)
    {
        this.audio = audio;
        this.position = position;
    }

}


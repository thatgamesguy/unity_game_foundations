using UnityEngine;

/// <summary>
/// Raised when audio clip should be played. Picked up by AudioPlayer2D.
/// </summary>
public class AudioEvent2D : GameEvent, IAudioEvent2D
{
    private AudioClip audioClip;
    public AudioClip Audio
    {
        get
        {
            return audioClip;
        }
    }

    public AudioEvent2D(AudioClip audioClip)
    {
        this.audioClip = audioClip;
    }
}


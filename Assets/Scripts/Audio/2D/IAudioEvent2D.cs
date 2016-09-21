using UnityEngine;

/// <summary>
/// Implemented by all audio events. Provides access to audio clip.
/// </summary>
public interface IAudioEvent2D
{
    AudioClip Audio { get; }
}


using UnityEngine;

/// <summary>
/// Listens to audio events. Adds audio events to circular array with MaxPending
/// and plays associated audio clip each time step.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer2D : MonoBehaviour
{
    /// <summary>
    /// The maximum number of queued clips. Oldest clips are overwritten when max reached.
    /// </summary>
    public int MaxPending = 30;

    private AudioSource source;

    /// <summary>
    /// Queue of events to play.
    /// </summary>
    private IAudioEvent2D[] pending;

    /// <summary>
    /// Reference to current head of cicular array (index to pop new audio event).
    /// </summary>
    private int head;

    /// <summary>
    /// Index to add new events.
    /// </summary>
    private int tail;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.spatialBlend = 0f;
    }

    void OnEnable()
    {
        head = tail = 0;

        pending = new IAudioEvent2D[MaxPending];

        Events.instance.AddListener<AudioEvent2D>(OnAudio);
    }

    void OnDisable()
    {
        Events.instance.RemoveListener<AudioEvent2D>(OnAudio);
    }

    /// <summary>
    /// Plays pending clips.
    /// </summary>
    void Update()
    {
        if (head == tail)
            return;

        Debug.Log("Playing AudioClip: " + pending[head].Audio.name);

        source.PlayOneShot(pending[head].Audio);

        head = (head + 1) % MaxPending;
    }

    void OnAudio(IAudioEvent2D e)
    {
        // Do not add duplicate events. Prevents situation where the same
        // audio clips are played in parallel increasing the effects volume.
        for (int i = head; i != tail; i = (i + 1) % MaxPending)
        {
            if (pending[i].Audio.name.Equals(e.Audio.name))
            {
                return;
            }
        }

        pending[tail] = e;
        tail = (tail + 1) % MaxPending;
    }

}


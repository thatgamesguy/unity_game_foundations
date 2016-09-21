using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class AudioPlayer2D : MonoBehaviour
{
    public int MaxPending = 30;

    private AudioSource source;
    private IAudioEvent2D[] pending;

    private int head;
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


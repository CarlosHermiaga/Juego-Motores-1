using System.Collections;
using UnityEngine;

public class BathroomAudioSequence : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip babyLaughClip;
    public AudioClip babyCryLoopClip;

    [Header("Timing")]
    public float delayBeforeCry = 0.7f;

    [Header("Settings")]
    public bool playOnStart = true;
    public bool playOnlyOnce = true;

    private bool hasPlayed = false;
    private Coroutine audioSequenceCoroutine;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (playOnStart)
        {
            PlaySequence();
        }
    }

    public void PlaySequence()
    {
        if (playOnlyOnce && hasPlayed)
        {
            return;
        }

        if (audioSequenceCoroutine != null)
        {
            StopCoroutine(audioSequenceCoroutine);
        }

        audioSequenceCoroutine = StartCoroutine(PlaySequenceRoutine());
    }

    private IEnumerator PlaySequenceRoutine()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("BathroomAudioSequence no tiene AudioSource asignado.");
            yield break;
        }

        hasPlayed = true;

        if (babyLaughClip != null)
        {
            audioSource.loop = false;
            audioSource.clip = babyLaughClip;
            audioSource.Play();

            yield return new WaitForSeconds(babyLaughClip.length);
        }

        yield return new WaitForSeconds(delayBeforeCry);

        if (babyCryLoopClip != null)
        {
            audioSource.loop = true;
            audioSource.clip = babyCryLoopClip;
            audioSource.Play();
        }
    }

    public void StopSequence()
    {
        if (audioSequenceCoroutine != null)
        {
            StopCoroutine(audioSequenceCoroutine);
            audioSequenceCoroutine = null;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
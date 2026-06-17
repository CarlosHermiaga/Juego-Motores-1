using System.Collections;
using UnityEngine;

public class BathtubBloodDrainInteractable : InteractableObject
{
    [Header("Bathroom Audio")]
    public BathroomAudioSequence bathroomAudioSequence;
    public GameObject bathroomAudioObject;
    public bool disableBathroomAudioObject = false;

    [Header("Blood Drain")]
    public GameObject bloodObject;
    public Transform drainPoint;
    public float bloodDrainDuration = 2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip drainSound;
    public AudioClip interactionSound;
    public float audioVolume = 1f;

    [Header("Cassette Reveal")]
    public GameObject cassetteToReveal;

    [Header("Interaction")]
    public Collider interactionCollider;
    public bool disableAfterInteraction = true;

    private bool hasInteracted = false;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (interactionCollider == null)
        {
            interactionCollider = GetComponent<Collider>();
        }

        if (cassetteToReveal != null)
        {
            cassetteToReveal.SetActive(false);
        }
    }

    public override void Interact()
    {
        if (hasInteracted)
        {
            return;
        }

        hasInteracted = true;

        Debug.Log("Interacción con la bañera.");

        StopBathroomAudio();
        PlayInteractionSound();

        StartCoroutine(DrainBloodRoutine());

        if (disableAfterInteraction && interactionCollider != null)
        {
            interactionCollider.enabled = false;
        }
    }

    private void StopBathroomAudio()
    {
        if (bathroomAudioSequence != null)
        {
            bathroomAudioSequence.StopSequence();
        }

        if (bathroomAudioObject != null && disableBathroomAudioObject)
        {
            bathroomAudioObject.SetActive(false);
        }
    }

    private void PlayInteractionSound()
    {
        if (audioSource == null)
        {
            return;
        }

        if (interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound, audioVolume);
        }

        if (drainSound != null)
        {
            audioSource.PlayOneShot(drainSound, audioVolume);
        }
    }

    private IEnumerator DrainBloodRoutine()
    {
        if (bloodObject == null)
        {
            RevealCassette();
            yield break;
        }

        Vector3 startScale = bloodObject.transform.localScale;
        Vector3 startPosition = bloodObject.transform.position;

        Vector3 targetScale = Vector3.zero;
        Vector3 targetPosition = startPosition + Vector3.down * 0.05f;

        if (drainPoint != null)
        {
            targetPosition = drainPoint.position;
        }

        float timer = 0f;

        while (timer < bloodDrainDuration)
        {
            timer += Time.deltaTime;

            float t = timer / bloodDrainDuration;

            bloodObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            bloodObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        bloodObject.SetActive(false);

        RevealCassette();
    }

    private void RevealCassette()
    {
        if (cassetteToReveal != null)
        {
            cassetteToReveal.SetActive(true);
        }
    }
}
using System.Collections;
using UnityEngine;

public class CassettePickup : InteractableObject
{
    [Header("Cassette")]
    public int cassetteIndex = 0;
    public string cassetteName = "Cassette";

    [Header("Audio")]
    public AudioSource pickupAudioSource;
    public AudioClip pickupSound;

    [Header("Visual")]
    public GameObject cassetteVisual;
    public Collider interactionCollider;

    [Header("Events On Pickup")]
    public GameObject[] objectsToActivateOnPickup;
    public GameObject[] objectsToDeactivateOnPickup;
    public AudioSource eventAudioSource;
    public AudioClip eventSound;

    private bool hasBeenPickedUp = false;

    private void Start()
    {
        if (pickupAudioSource == null)
        {
            pickupAudioSource = GetComponent<AudioSource>();
        }

        if (cassetteVisual == null)
        {
            cassetteVisual = gameObject;
        }

        if (interactionCollider == null)
        {
            interactionCollider = GetComponent<Collider>();
        }
    }

    public override void Interact()
    {
        if (hasBeenPickedUp)
        {
            return;
        }

        hasBeenPickedUp = true;

        PlayerObjectiveInventory inventory = FindObjectOfType<PlayerObjectiveInventory>();

        if (inventory != null)
        {
            inventory.CollectCassette(cassetteIndex);
        }
        else
        {
            Debug.LogWarning("No se encontró PlayerObjectiveInventory en el Player.");
        }

        ActivatePickupEvents();

        if (interactionCollider != null)
        {
            interactionCollider.enabled = false;
        }

        SetRenderersEnabled(false);

        if (pickupAudioSource != null && pickupSound != null)
        {
            pickupAudioSource.PlayOneShot(pickupSound);
            StartCoroutine(DestroyAfterSound());
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log("Agarraste: " + cassetteName);
    }

    private void ActivatePickupEvents()
    {
        if (objectsToActivateOnPickup != null)
        {
            foreach (GameObject objectToActivate in objectsToActivateOnPickup)
            {
                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                }
            }
        }

        if (objectsToDeactivateOnPickup != null)
        {
            foreach (GameObject objectToDeactivate in objectsToDeactivateOnPickup)
            {
                if (objectToDeactivate != null)
                {
                    objectToDeactivate.SetActive(false);
                }
            }
        }

        if (eventAudioSource != null && eventSound != null)
        {
            eventAudioSource.PlayOneShot(eventSound);
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(pickupSound.length);
        Destroy(gameObject);
    }

    private void SetRenderersEnabled(bool enabled)
    {
        Renderer[] renderers = cassetteVisual.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }
}
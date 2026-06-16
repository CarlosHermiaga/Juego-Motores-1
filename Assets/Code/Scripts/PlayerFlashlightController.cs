using UnityEngine;

public class PlayerFlashlightController : MonoBehaviour
{
    [Header("Flashlight")]
    public GameObject flashlightObject;
    public bool hasFlashlight = false;
    public KeyCode toggleKey = KeyCode.F;

    [Header("Audio")]
    public AudioSource flashlightAudioSource;
    public AudioClip pickupClickSound;
    public AudioClip toggleClickSound;
    public float audioVolume = 1f;

    private bool isOn = false;

    private void Start()
    {
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (hasFlashlight && Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    public void PickUpFlashlight()
    {
        hasFlashlight = true;
        isOn = true;

        if (flashlightObject != null)
        {
            flashlightObject.SetActive(true);
        }

        PlaySound(pickupClickSound);

        Debug.Log("Agarraste la linterna");
    }

    public void ToggleFlashlight()
    {
        isOn = !isOn;

        if (flashlightObject != null)
        {
            flashlightObject.SetActive(isOn);
        }

        if (toggleClickSound != null)
        {
            PlaySound(toggleClickSound);
        }
        else
        {
            PlaySound(pickupClickSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (flashlightAudioSource == null || clip == null)
        {
            return;
        }

        flashlightAudioSource.pitch = 1f;
        flashlightAudioSource.PlayOneShot(clip, audioVolume);
    }
}
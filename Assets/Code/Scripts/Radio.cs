using UnityEngine;

public class Radio : InteractableObject
{
    [Header("Radio State")]
    public bool isOn = false;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Settings")]
    public bool startOff = true;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null && startOff)
        {
            isOn = false;
            audioSource.playOnAwake = false;
            audioSource.Stop();
        }
    }

    public override void Interact()
    {
        isOn = !isOn;

        if (audioSource == null)
        {
            Debug.LogWarning("La radio no tiene AudioSource asignado.");
            return;
        }

        if (isOn)
        {
            audioSource.Play();
            Debug.Log("La radio se prendió");
        }
        else
        {
            audioSource.Stop();
            Debug.Log("La radio se apagó");
        }
    }
}
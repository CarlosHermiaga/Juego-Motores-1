using UnityEngine;

public class Radio : InteractableObject
{
    [Header("Audio Source")]
    public AudioSource radioAudioSource;

    [Header("Static Radio")]
    public AudioClip staticClip;
    public bool allowStaticIfNoCassette = true;

    [Header("Cassette Player")]
    public AudioClip[] cassetteNewsClips;

    [Header("Settings")]
    public bool startOff = true;

    private bool isStaticOn = false;
    private bool isPlayingCassette = false;

    private void Start()
    {
        if (radioAudioSource == null)
        {
            radioAudioSource = GetComponent<AudioSource>();
        }

        if (radioAudioSource != null && startOff)
        {
            radioAudioSource.playOnAwake = false;
            radioAudioSource.Stop();
        }
    }

    public override void Interact()
    {
        PlayerObjectiveInventory inventory = FindObjectOfType<PlayerObjectiveInventory>();

        if (isPlayingCassette && radioAudioSource != null && radioAudioSource.isPlaying)
        {
            StopRadio();
            return;
        }

        if (inventory != null && inventory.HasAnyCassette())
        {
            PlayNextCassette(inventory);
            return;
        }

        if (allowStaticIfNoCassette)
        {
            ToggleStatic();
        }
        else
        {
            Debug.Log("No hay cassettes para reproducir.");
        }
    }

    private void ToggleStatic()
    {
        if (radioAudioSource == null)
        {
            Debug.LogWarning("La radio no tiene AudioSource asignado.");
            return;
        }

        if (staticClip == null)
        {
            Debug.LogWarning("No hay sonido de estática asignado.");
            return;
        }

        if (isStaticOn)
        {
            StopRadio();
            return;
        }

        isStaticOn = true;
        isPlayingCassette = false;

        radioAudioSource.Stop();
        radioAudioSource.clip = staticClip;
        radioAudioSource.loop = true;
        radioAudioSource.Play();

        Debug.Log("La radio se prendió.");
    }

    private void PlayNextCassette(PlayerObjectiveInventory inventory)
    {
        int nextCassetteIndex = inventory.GetNextUnplayedCassetteIndex();

        if (nextCassetteIndex == -1)
        {
            nextCassetteIndex = inventory.GetFirstCollectedCassetteIndex();
        }

        if (nextCassetteIndex == -1)
        {
            Debug.Log("No hay cassettes disponibles.");
            return;
        }

        if (cassetteNewsClips == null || nextCassetteIndex >= cassetteNewsClips.Length)
        {
            Debug.LogWarning("No hay AudioClip asignado para el cassette " + nextCassetteIndex);
            return;
        }

        AudioClip selectedClip = cassetteNewsClips[nextCassetteIndex];

        if (selectedClip == null)
        {
            Debug.LogWarning("El clip del cassette " + nextCassetteIndex + " está vacío.");
            return;
        }

        isStaticOn = false;
        isPlayingCassette = true;

        radioAudioSource.Stop();
        radioAudioSource.clip = selectedClip;
        radioAudioSource.loop = false;
        radioAudioSource.Play();

        inventory.MarkCassetteAsPlayed(nextCassetteIndex);

        Debug.Log("Reproduciendo cassette " + (nextCassetteIndex + 1));
    }

    private void StopRadio()
    {
        if (radioAudioSource != null)
        {
            radioAudioSource.Stop();
        }

        isStaticOn = false;
        isPlayingCassette = false;

        Debug.Log("Radio detenida.");
    }
}
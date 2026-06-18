using System.Collections;
using UnityEngine;

public class Radio : InteractableObject
{
    [Header("Audio Source")]
    public AudioSource radioAudioSource;

    [Header("Static Radio")]
    public AudioClip staticClip;
    public bool startWithStatic = false;
    public bool startStaticWhenEnabled = true;
    public bool interactionDoesNothingWithoutCassette = true;

    [Header("Cassette Player")]
    public AudioClip[] cassetteNewsClips;
    public bool returnToStaticAfterCassette = true;
    public bool canStopCassetteWithInteraction = false;

    private bool isStaticOn = false;
    private bool isPlayingCassette = false;
    private Coroutine cassetteCoroutine;
    private Coroutine startStaticCoroutine;

    private void Awake()
    {
        SetupRadio();
    }

    private void Start()
    {
        SetupRadio();

        if (startWithStatic)
        {
            StartStatic();
        }
    }

    private void OnEnable()
    {
        SetupRadio();

        if (startStaticWhenEnabled)
        {
            if (startStaticCoroutine != null)
            {
                StopCoroutine(startStaticCoroutine);
            }

            startStaticCoroutine = StartCoroutine(StartStaticAfterOneFrame());
        }
    }

    private IEnumerator StartStaticAfterOneFrame()
    {
        yield return null;
        StartStatic();
    }

    private void SetupRadio()
    {
        if (radioAudioSource == null)
        {
            radioAudioSource = GetComponent<AudioSource>();
        }

        if (radioAudioSource != null)
        {
            radioAudioSource.playOnAwake = false;
        }
    }

    public override void Interact()
    {
        PlayerObjectiveInventory inventory = FindObjectOfType<PlayerObjectiveInventory>();

        if (isPlayingCassette)
        {
            if (canStopCassetteWithInteraction)
            {
                StopRadio();
            }

            return;
        }

        if (inventory != null && inventory.HasAnyCassette())
        {
            PlayNextCassette(inventory);
            return;
        }

        if (interactionDoesNothingWithoutCassette)
        {
            Debug.Log("La radio solo emite estática.");
            return;
        }

        ToggleStatic();
    }

    private void StartStatic()
    {
        if (radioAudioSource == null)
        {
            Debug.LogWarning("La radio no tiene AudioSource asignado.");
            return;
        }

        if (staticClip == null)
        {
            Debug.LogWarning("La radio no tiene Static Clip asignado.");
            return;
        }

        isStaticOn = true;
        isPlayingCassette = false;

        radioAudioSource.Stop();
        radioAudioSource.clip = staticClip;
        radioAudioSource.loop = true;
        radioAudioSource.Play();

        Debug.Log("Radio prendida con estática.");
    }

    private void ToggleStatic()
    {
        if (isStaticOn)
        {
            StopRadio();
        }
        else
        {
            StartStatic();
        }
    }

    private void PlayNextCassette(PlayerObjectiveInventory inventory)
    {
        int nextCassetteIndex = inventory.GetNextUnplayedCassetteIndex();

        if (nextCassetteIndex == -1)
        {
            Debug.Log("No hay cassettes nuevos para reproducir.");
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

        Debug.Log("Reproduciendo cassette " + (nextCassetteIndex + 1));

        if (cassetteCoroutine != null)
        {
            StopCoroutine(cassetteCoroutine);
        }

        cassetteCoroutine = StartCoroutine(WaitForCassetteToEnd(nextCassetteIndex, selectedClip.length, inventory));
    }

    private IEnumerator WaitForCassetteToEnd(int cassetteIndex, float cassetteLength, PlayerObjectiveInventory inventory)
    {
        yield return new WaitForSeconds(cassetteLength);

        isPlayingCassette = false;

        if (inventory != null)
        {
            inventory.MarkCassetteAsPlayed(cassetteIndex);
        }

        if (returnToStaticAfterCassette)
        {
            StartStatic();
        }
    }

    private void StopRadio()
    {
        if (cassetteCoroutine != null)
        {
            StopCoroutine(cassetteCoroutine);
            cassetteCoroutine = null;
        }

        if (radioAudioSource != null)
        {
            radioAudioSource.Stop();
        }

        isStaticOn = false;
        isPlayingCassette = false;

        Debug.Log("Radio detenida.");
    }
}
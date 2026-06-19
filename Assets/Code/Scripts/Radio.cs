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
    public int cassetteIndexToPlay = 0;
    public bool returnToStaticAfterCassette = true;
    public bool canStopCassetteWithInteraction = false;

    [Header("Door Unlock")]
    public bool unlockDoorWhenCassetteStarts = false;
    public int cassetteIndexThatUnlocksDoor = 0;
    public DoorSystem loop1DoorToUnlock;

    private bool isStaticOn = false;
    private bool isPlayingCassette = false;
    private bool hasUnlockedDoor = false;

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

        if (inventory != null && inventory.HasCassette(cassetteIndexToPlay))
        {
            PlayCassette(cassetteIndexToPlay, inventory);
            return;
        }

        if (interactionDoesNothingWithoutCassette)
        {
            Debug.Log("La radio solo emite estática. Falta el cassette " + (cassetteIndexToPlay + 1) + ".");
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

    private void PlayCassette(int cassetteIndex, PlayerObjectiveInventory inventory)
    {
        if (cassetteNewsClips == null || cassetteIndex >= cassetteNewsClips.Length)
        {
            Debug.LogWarning("No hay AudioClip asignado para el cassette " + cassetteIndex);
            return;
        }

        AudioClip selectedClip = cassetteNewsClips[cassetteIndex];

        if (selectedClip == null)
        {
            Debug.LogWarning("El clip del cassette " + cassetteIndex + " está vacío.");
            return;
        }

        isStaticOn = false;
        isPlayingCassette = true;

        radioAudioSource.Stop();
        radioAudioSource.clip = selectedClip;
        radioAudioSource.loop = false;
        radioAudioSource.Play();

        UnlockDoorIfNeeded(cassetteIndex);

        Debug.Log("Reproduciendo cassette " + (cassetteIndex + 1));

        if (cassetteCoroutine != null)
        {
            StopCoroutine(cassetteCoroutine);
        }

        cassetteCoroutine = StartCoroutine(WaitForCassetteToEnd(cassetteIndex, selectedClip.length, inventory));
    }

    private void UnlockDoorIfNeeded(int cassetteIndex)
    {
        if (!unlockDoorWhenCassetteStarts)
        {
            return;
        }

        if (hasUnlockedDoor)
        {
            return;
        }

        if (cassetteIndex != cassetteIndexThatUnlocksDoor)
        {
            return;
        }

        if (loop1DoorToUnlock == null)
        {
            Debug.LogWarning("No hay puerta asignada para destrabar.");
            return;
        }

        hasUnlockedDoor = true;
        loop1DoorToUnlock.UnlockDoor();

        Debug.Log("Puerta destrabada por cassette.");
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
using UnityEngine;

public class PlayerFootstepController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips;

    [Header("Step Timing")]
    public float minStepInterval = 0.68f;
    public float maxStepInterval = 0.88f;

    [Header("Volume")]
    public float baseVolume = 0.3f;
    public float volumeVariation = 0.06f;

    [Header("Pitch")]
    public float minPitch = 0.99f;
    public float maxPitch = 1.01f;

    [Header("Debug")]
    public bool showDebug = false;

    private float stepTimer = 0f;
    private AudioClip lastClip;

    private void Start()
    {
        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }

        stepTimer = Random.Range(minStepInterval, maxStepInterval);

        if (footstepAudioSource == null)
        {
            Debug.LogError("No hay AudioSource en el Player para reproducir pasos.");
        }
    }

    private void Update()
    {
        if (footstepAudioSource == null)
        {
            return;
        }

        bool isMoving =
            Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0;

        if (!isMoving)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = Random.Range(minStepInterval, maxStepInterval);
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0)
        {
            Debug.LogWarning("No hay clips de pasos cargados en Footstep Clips.");
            return;
        }

        AudioClip selectedClip = GetRandomClip();

        if (selectedClip == null)
        {
            return;
        }

        float randomVolume = Random.Range(baseVolume - volumeVariation, baseVolume + volumeVariation);
        randomVolume = Mathf.Clamp01(randomVolume);

        footstepAudioSource.pitch = Random.Range(minPitch, maxPitch);
        footstepAudioSource.PlayOneShot(selectedClip, randomVolume);

        if (showDebug)
        {
            Debug.Log("Paso reproducido: " + selectedClip.name);
        }

        lastClip = selectedClip;
    }

    private AudioClip GetRandomClip()
    {
        if (footstepClips.Length == 1)
        {
            return footstepClips[0];
        }

        AudioClip selectedClip = footstepClips[Random.Range(0, footstepClips.Length)];

        int safetyCounter = 0;

        while (selectedClip == lastClip && safetyCounter < 10)
        {
            selectedClip = footstepClips[Random.Range(0, footstepClips.Length)];
            safetyCounter++;
        }

        return selectedClip;
    }
}
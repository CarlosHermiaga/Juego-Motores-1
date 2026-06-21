using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CassetteEndLoopTransition : MonoBehaviour
{
    [Header("Cassette Detection")]
    public AudioSource radioAudioSource;
    public AudioClip cassetteToDetect;

    [Header("Player")]
    public GameObject player;

    [Header("Timing")]
    public float delayAfterCassetteEnds = 1.5f;
    public float weirdEffectDuration = 2f;

    [Header("Audio Event")]
    public AudioSource eventAudioSource;
    public AudioClip eventSound;
    public float eventVolume = 1f;

    [Header("Camera Effect")]
    public Camera playerCamera;
    public float shakeAmount = 0.08f;
    public float shakeSpeed = 35f;
    public float fovIncrease = 10f;

    [Header("Visual Glitch Objects")]
    public GameObject[] objectsToActivateDuringEffect;
    public GameObject[] objectsToDeactivateAfterEffect;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeOutDuration = 0.6f;
    public float blackScreenDuration = 0.25f;
    public float fadeInDuration = 0.7f;

    [Header("Loop")]
    public bool advanceLoop = true;

    private bool cassetteStarted = false;
    private bool transitionStarted = false;

    private Vector3 originalCameraLocalPosition;
    private float originalFov;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject;
            }
        }

        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main;
        }

        if (playerCamera != null)
        {
            originalCameraLocalPosition = playerCamera.transform.localPosition;
            originalFov = playerCamera.fieldOfView;
        }

        ClearFade();
        SetEffectObjects(false);
    }

    private void Update()
    {
        if (transitionStarted)
        {
            return;
        }

        if (radioAudioSource == null || cassetteToDetect == null)
        {
            return;
        }

        if (!cassetteStarted)
        {
            if (radioAudioSource.isPlaying && radioAudioSource.clip == cassetteToDetect)
            {
                cassetteStarted = true;
                Debug.Log("Cassette detectado: empezó la reproducción.");
            }

            return;
        }

        bool cassetteNoLongerPlaying = !radioAudioSource.isPlaying || radioAudioSource.clip != cassetteToDetect;

        if (cassetteNoLongerPlaying)
        {
            transitionStarted = true;
            StartCoroutine(TransitionRoutine());
        }
    }

    private IEnumerator TransitionRoutine()
    {
        Debug.Log("Cassette terminó. Iniciando transición al loop 3.");

        yield return new WaitForSeconds(delayAfterCassetteEnds);

        if (eventAudioSource != null && eventSound != null)
        {
            eventAudioSource.PlayOneShot(eventSound, eventVolume);
        }

        SetEffectObjects(true);

        yield return StartCoroutine(CameraWeirdEffect());

        yield return StartCoroutine(FadeToAlpha(1f, fadeOutDuration));

        if (advanceLoop && LoopManager.Instance != null && player != null)
        {
            LoopManager.Instance.AdvanceLoop(player);
        }

        yield return new WaitForSeconds(blackScreenDuration);

        SetEffectObjects(false);

        yield return StartCoroutine(FadeToAlpha(0f, fadeInDuration));
    }

    private IEnumerator CameraWeirdEffect()
    {
        if (playerCamera == null)
        {
            yield return new WaitForSeconds(weirdEffectDuration);
            yield break;
        }

        float timer = 0f;

        while (timer < weirdEffectDuration)
        {
            timer += Time.deltaTime;

            float intensity = 1f - Mathf.Clamp01(timer / weirdEffectDuration);

            float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * shakeAmount * intensity;
            float offsetY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * shakeAmount * intensity;

            playerCamera.transform.localPosition = originalCameraLocalPosition + new Vector3(offsetX, offsetY, 0f);
            playerCamera.fieldOfView = originalFov + fovIncrease * intensity;

            yield return null;
        }

        playerCamera.transform.localPosition = originalCameraLocalPosition;
        playerCamera.fieldOfView = originalFov;
    }

    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        if (fadeImage == null)
        {
            yield break;
        }

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = false;

        Color color = fadeImage.color;
        float startAlpha = color.a;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }

    private void ClearFade()
    {
        if (fadeImage == null)
        {
            return;
        }

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = false;

        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
    }

    private void SetEffectObjects(bool active)
    {
        if (objectsToActivateDuringEffect != null)
        {
            foreach (GameObject obj in objectsToActivateDuringEffect)
            {
                if (obj != null)
                {
                    obj.SetActive(active);
                }
            }
        }

        if (!active && objectsToDeactivateAfterEffect != null)
        {
            foreach (GameObject obj in objectsToDeactivateAfterEffect)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
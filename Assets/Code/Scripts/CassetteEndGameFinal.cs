using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CassetteEndGameFinal : MonoBehaviour
{
    [Header("Cassette Detection")]
    public AudioSource radioAudioSource;
    public AudioClip finalCassetteClip;

    [Header("Timing")]
    public float delayAfterCassetteEnds = 1.2f;

    [Header("Audio")]
    public AudioSource eventAudioSource;
    public AudioClip finalSound;
    public float finalSoundVolume = 1f;

    [Header("Flash")]
    public Image flashImage;
    public float flashInDuration = 0.06f;
    public float flashHoldDuration = 0.4f;
    public float flashOutDuration = 1f;
    public bool stayWhiteAtEnd = true;

    [Header("Final Text")]
    public GameObject finalText;
    public float finalTextDelay = 0.6f;

    [Header("End Game")]
    public bool freezeGameAtEnd = true;

    private bool cassetteStarted = false;
    private bool finalStarted = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (eventAudioSource == null)
        {
            eventAudioSource = GetComponent<AudioSource>();
        }

        ClearFlash();

        if (finalText != null)
        {
            finalText.SetActive(false);
        }
    }

    private void Update()
    {
        if (finalStarted)
        {
            return;
        }

        if (radioAudioSource == null || finalCassetteClip == null)
        {
            return;
        }

        if (!cassetteStarted)
        {
            if (radioAudioSource.isPlaying && radioAudioSource.clip == finalCassetteClip)
            {
                cassetteStarted = true;
                Debug.Log("Cassette final detectado.");
            }

            return;
        }

        bool cassetteEnded = !radioAudioSource.isPlaying || radioAudioSource.clip != finalCassetteClip;

        if (cassetteEnded)
        {
            finalStarted = true;
            StartCoroutine(FinalRoutine());
        }
    }

    private IEnumerator FinalRoutine()
    {
        Debug.Log("Cassette final terminado. Iniciando final.");

        yield return new WaitForSeconds(delayAfterCassetteEnds);

        if (eventAudioSource != null && finalSound != null)
        {
            eventAudioSource.PlayOneShot(finalSound, finalSoundVolume);
        }

        yield return StartCoroutine(FlashRoutine());

        yield return new WaitForSeconds(finalTextDelay);

        if (finalText != null)
        {
            finalText.SetActive(true);
        }

        if (freezeGameAtEnd)
        {
            Time.timeScale = 0f;
        }
    }

    private IEnumerator FlashRoutine()
    {
        yield return StartCoroutine(FadeFlashToAlpha(1f, flashInDuration));

        yield return new WaitForSeconds(flashHoldDuration);

        if (stayWhiteAtEnd)
        {
            SetFlashAlpha(1f);
        }
        else
        {
            yield return StartCoroutine(FadeFlashToAlpha(0f, flashOutDuration));
        }
    }

    private IEnumerator FadeFlashToAlpha(float targetAlpha, float duration)
    {
        if (flashImage == null)
        {
            yield break;
        }

        flashImage.gameObject.SetActive(true);
        flashImage.raycastTarget = false;

        Color color = flashImage.color;
        float startAlpha = color.a;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            flashImage.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        flashImage.color = color;
    }

    private void ClearFlash()
    {
        SetFlashAlpha(0f);
    }

    private void SetFlashAlpha(float alpha)
    {
        if (flashImage == null)
        {
            return;
        }

        flashImage.gameObject.SetActive(true);
        flashImage.raycastTarget = false;

        Color color = flashImage.color;
        color.a = alpha;
        flashImage.color = color;
    }
}
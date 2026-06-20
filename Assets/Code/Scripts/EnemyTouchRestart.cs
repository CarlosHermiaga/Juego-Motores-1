using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyTouchRestart : MonoBehaviour
{
    [Header("Restart Settings")]
    public float restartDelay = 0.4f;
    public bool restartOnlyOnce = true;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 0.6f;

    [Header("Boost Current Enemy Audio")]
    public AudioSource enemyAudioSource;
    public float boostedVolume = 1f;
    public float volumeBoostDuration = 0.15f;

    private bool hasTriggered = false;

    private void Start()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        TriggerRestart();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        TriggerRestart();
    }

    private void TriggerRestart()
    {
        if (restartOnlyOnce && hasTriggered)
        {
            return;
        }

        hasTriggered = true;

        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        StartCoroutine(BoostCurrentAudioVolume());

        yield return StartCoroutine(FadeToBlack());

        yield return new WaitForSeconds(restartDelay);

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private IEnumerator BoostCurrentAudioVolume()
    {
        if (enemyAudioSource == null)
        {
            yield break;
        }

        float startVolume = enemyAudioSource.volume;
        float timer = 0f;

        while (timer < volumeBoostDuration)
        {
            timer += Time.deltaTime;

            float t = timer / volumeBoostDuration;
            enemyAudioSource.volume = Mathf.Lerp(startVolume, boostedVolume, t);

            yield return null;
        }

        enemyAudioSource.volume = boostedVolume;
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeImage == null)
        {
            yield break;
        }

        float timer = 0f;

        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = timer / fadeDuration;
            alpha = Mathf.Clamp01(alpha);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}
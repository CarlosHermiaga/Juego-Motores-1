using UnityEngine;
using TMPro;

public class FinalTextGlitch : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI finalText;

    [Header("Position Glitch")]
    public float positionAmount = 12f;
    public float rotationAmount = 4f;
    public float scaleAmount = 0.08f;

    [Header("Timing")]
    public float glitchInterval = 0.06f;
    public float returnSpeed = 10f;

    [Header("Flicker")]
    public bool useFlicker = true;
    public float minAlpha = 0.45f;
    public float maxAlpha = 1f;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Vector3 originalLocalScale;

    private float timer = 0f;

    private void Awake()
    {
        if (finalText == null)
        {
            finalText = GetComponent<TextMeshProUGUI>();
        }

        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        originalLocalScale = transform.localScale;
    }

    private void OnEnable()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        originalLocalScale = transform.localScale;

        timer = 0f;
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= glitchInterval)
        {
            timer = 0f;
            ApplyGlitch();
        }

        ReturnToOriginal();
    }

    private void ApplyGlitch()
    {
        float randomX = Random.Range(-positionAmount, positionAmount);
        float randomY = Random.Range(-positionAmount, positionAmount);
        float randomRotZ = Random.Range(-rotationAmount, rotationAmount);
        float randomScale = 1f + Random.Range(-scaleAmount, scaleAmount);

        transform.localPosition = originalLocalPosition + new Vector3(randomX, randomY, 0f);
        transform.localRotation = originalLocalRotation * Quaternion.Euler(0f, 0f, randomRotZ);
        transform.localScale = originalLocalScale * randomScale;

        if (useFlicker && finalText != null)
        {
            Color color = finalText.color;
            color.a = Random.Range(minAlpha, maxAlpha);
            finalText.color = color;
        }
    }

    private void ReturnToOriginal()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            originalLocalPosition,
            Time.unscaledDeltaTime * returnSpeed
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            originalLocalRotation,
            Time.unscaledDeltaTime * returnSpeed
        );

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            originalLocalScale,
            Time.unscaledDeltaTime * returnSpeed
        );
    }
}
using UnityEngine;

public class EnemyPresence : MonoBehaviour
{
    [Header("Look At Player")]
    public Transform player;
    public bool lookAtPlayer = true;
    public bool rotateOnlyOnY = true;
    public float rotationSpeed = 5f;
    public float rotationOffsetY = 0f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip appearSound;
    public float appearVolume = 1f;

    private void OnEnable()
    {
        FindPlayerIfNeeded();
        PlayAppearSound();
    }

    private void Update()
    {
        if (!lookAtPlayer)
        {
            return;
        }

        if (player == null)
        {
            FindPlayerIfNeeded();
            return;
        }

        LookAtPlayer();
    }

    private void FindPlayerIfNeeded()
    {
        if (player != null)
        {
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            return;
        }

        if (Camera.main != null)
        {
            player = Camera.main.transform;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;

        if (rotateOnlyOnY)
        {
            direction.y = 0f;
        }

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation *= Quaternion.Euler(0f, rotationOffsetY, 0f);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void PlayAppearSound()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null && appearSound != null)
        {
            audioSource.PlayOneShot(appearSound, appearVolume);
        }
    }
}
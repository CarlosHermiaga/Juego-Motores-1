using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPresence : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Look At Player")]
    public bool lookAtPlayer = true;
    public bool rotateOnlyOnY = true;
    public float rotationSpeed = 5f;
    public float rotationOffsetY = 0f;

    [Header("Chase")]
    public bool canChase = true;
    public float chaseStartDistance = 6f;
    public float chaseStopDistance = 12f;
    public float minDistanceToPlayer = 1.4f;

    [Header("Safe Room")]
    public SafeRoomZone safeRoomZone;
    public bool stopChaseWhenPlayerIsSafe = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip appearSound;
    public AudioClip chaseStartSound;
    public float appearVolume = 1f;
    public float chaseStartVolume = 1f;

    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool playedChaseSound = false;

    [SerializeField] Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (agent != null)
        {
            agent.isStopped = true;
            agent.updateRotation = false;
        }
    }

    private void OnEnable()
    {
        FindPlayerIfNeeded();
        PlayAppearSound();

        isChasing = false;
        playedChaseSound = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    private void Update()
    {
        FindPlayerIfNeeded();

        if (player == null)
        {
            return;
        }

        if (stopChaseWhenPlayerIsSafe && safeRoomZone != null && safeRoomZone.PlayerIsInside)
        {
            StopChase();
            LookAtPlayer();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (canChase && !isChasing && distanceToPlayer <= chaseStartDistance)
        {
            StartChase();
        }

        if (isChasing)
        {
            if (distanceToPlayer >= chaseStopDistance)
            {
                StopChase();
            }
            else
            {
                ChasePlayer(distanceToPlayer);
            }
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
        }
        else if (Camera.main != null)
        {
            player = Camera.main.transform;
        }
    }

    private void StartChase()
    {
        isChasing = true;
        animator.SetBool("Chasing", true);

        if (agent != null)
        {
            agent.isStopped = false;
        }

        if (!playedChaseSound && audioSource != null && chaseStartSound != null)
        {
            audioSource.PlayOneShot(chaseStartSound, chaseStartVolume);
            playedChaseSound = true;
        }

        Debug.Log("El enemigo empezó a seguir al jugador.");
    }

    private void StopChase()
    {
        if (!isChasing)
        {
            return;
        }

        isChasing = false;
        animator.SetBool("Chasing", false);

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        Debug.Log("El enemigo dejó de perseguir.");
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= minDistanceToPlayer)
        {
            if (agent != null)
            {
                agent.isStopped = true;
            }

            return;
        }

        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    private void LookAtPlayer()
    {
        if (!lookAtPlayer || player == null)
        {
            return;
        }

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
        if (audioSource != null && appearSound != null)
        {
            audioSource.PlayOneShot(appearSound, appearVolume);
        }
    }
}
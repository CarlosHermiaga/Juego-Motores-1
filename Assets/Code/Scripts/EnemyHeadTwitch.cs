using UnityEngine;
using UnityEngine.AI;

public class EnemyHeadTwitch : MonoBehaviour
{
    [Header("References")]
    public Transform headTransform;
    public NavMeshAgent agent;
    public Transform player;

    [Header("Activation")]
    public bool twitchOnlyWhenMoving = true;
    public float activationDistance = 8f;
    public float movementThreshold = 0.1f;

    [Header("Twitch Settings")]
    public float twitchInterval = 0.06f;
    public float snapSpeed = 900f;

    [Header("Rotation Intensity")]
    public float maxPitch = 25f;
    public float maxYaw = 35f;
    public float maxRoll = 25f;

    [Header("Return")]
    public float returnSpeed = 8f;

    private Quaternion initialLocalRotation;
    private Quaternion targetLocalRotation;
    private float twitchTimer;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        FindPlayerIfNeeded();

        if (headTransform != null)
        {
            initialLocalRotation = headTransform.localRotation;
            targetLocalRotation = initialLocalRotation;
        }
    }

    private void OnEnable()
    {
        FindPlayerIfNeeded();

        if (headTransform != null)
        {
            initialLocalRotation = headTransform.localRotation;
            targetLocalRotation = initialLocalRotation;
        }

        twitchTimer = 0f;
    }

    private void LateUpdate()
    {
        if (headTransform == null)
        {
            return;
        }

        FindPlayerIfNeeded();

        if (ShouldTwitch())
        {
            TwitchHead();
        }
        else
        {
            ReturnHeadToNormal();
        }
    }

    private bool ShouldTwitch()
    {
        if (player == null)
        {
            return false;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > activationDistance)
        {
            return false;
        }

        if (!twitchOnlyWhenMoving)
        {
            return true;
        }

        if (agent == null)
        {
            return true;
        }

        return agent.velocity.magnitude > movementThreshold;
    }

    private void TwitchHead()
    {
        twitchTimer -= Time.deltaTime;

        if (twitchTimer <= 0f)
        {
            twitchTimer = twitchInterval;

            float randomPitch = Random.Range(-maxPitch, maxPitch);
            float randomYaw = Random.Range(-maxYaw, maxYaw);
            float randomRoll = Random.Range(-maxRoll, maxRoll);

            targetLocalRotation = initialLocalRotation * Quaternion.Euler(randomPitch, randomYaw, randomRoll);
        }

        headTransform.localRotation = Quaternion.RotateTowards(
            headTransform.localRotation,
            targetLocalRotation,
            snapSpeed * Time.deltaTime
        );
    }

    private void ReturnHeadToNormal()
    {
        headTransform.localRotation = Quaternion.Slerp(
            headTransform.localRotation,
            initialLocalRotation,
            returnSpeed * Time.deltaTime
        );
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
}
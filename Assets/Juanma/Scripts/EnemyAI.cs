using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("Detección")]
    public float viewDistance = 10f;

    private NavMeshAgent agent;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= viewDistance)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
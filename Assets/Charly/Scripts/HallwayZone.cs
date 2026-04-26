using UnityEngine;

public class HallwayZone : MonoBehaviour
{
    [SerializeField] InfiniteEffects fovEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fovEffect.SetInHallway(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fovEffect.SetInHallway(false);
        }
    }
}

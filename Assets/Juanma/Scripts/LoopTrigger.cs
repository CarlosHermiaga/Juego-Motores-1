using UnityEngine;

public class LoopTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            LoopManager.Instance.AdvanceLoop(other.gameObject);
            Invoke(nameof(ResetTrigger), 0.5f);
        }
    }

    private void ResetTrigger()
    {
        triggered = false;
    }
}
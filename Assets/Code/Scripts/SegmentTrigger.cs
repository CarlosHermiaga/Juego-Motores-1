using UnityEngine;

public class SegmentTrigger : MonoBehaviour
{
    private HallwayGenerator generator;

    private bool activated;

    private void Start()
    {
        generator = GetComponentInParent<HallwayGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            generator.SpawnSegment();
        }
    }
}
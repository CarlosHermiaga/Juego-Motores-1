using UnityEngine;

public class PlaceOnEnableAtTransform : MonoBehaviour
{
    public Transform targetTransform;

    private void OnEnable()
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("No hay Target Transform asignado para aparecer.");
            return;
        }

        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
}
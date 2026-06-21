using UnityEngine;

public class SegmentDebug : MonoBehaviour
{
    public Transform entrada;
    public Transform salida;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(entrada.position, 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(salida.position, 1f);
    }
}
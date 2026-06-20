using UnityEngine;

public class SafeRoomZone : MonoBehaviour
{
    public bool PlayerIsInside { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIsInside = true;
            Debug.Log("Jugador entró a la zona segura.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIsInside = false;
            Debug.Log("Jugador salió de la zona segura.");
        }
    }
}
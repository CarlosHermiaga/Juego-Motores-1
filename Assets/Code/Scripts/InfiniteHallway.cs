using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InfiniteHallway : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform teleportPoint;

    bool canTeleport = true;

    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            canTeleport = false;
        }
        else
        {
            canTeleport = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport) return;

        if (other.transform == player)
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        CharacterController characterController = player.GetComponent<CharacterController>();

        if (characterController != null)
            characterController.enabled = false;

        player.position = teleportPoint.position;

        if (characterController != null)
            characterController.enabled = true;
    }
}

using System.Collections;
using UnityEngine;

public class DoorInteractable : InteractableObject
{
    [Header("Apertura")]
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);
    public float openDuration = 0.6f;
    public bool canClose = true;

    [Header("Bloqueo")]
    public bool isLocked = false;
    public bool requiresAllItems = false;

    [Header("Collider")]
    public Collider doorCollider;
    public bool disableColliderWhenOpen = false;

    [Header("Mensajes")]
    public string lockedMessage = "La puerta está cerrada.";
    public string missingItemsMessage = "Parece que todavía falta algo...";
    public string openMessage = "La puerta se abre.";
    public string closeMessage = "La puerta se cierra.";

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        closedRotation = transform.rotation;
        openedRotation = Quaternion.Euler(transform.eulerAngles + openRotation);

        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider>();
        }
    }

    public override void Interact()
    {
        if (isMoving)
        {
            return;
        }

        if (isOpen)
        {
            if (canClose)
            {
                StartCoroutine(RotateDoor(closedRotation, false));
            }

            return;
        }

        if (!CanOpenDoor())
        {
            return;
        }

        StartCoroutine(RotateDoor(openedRotation, true));
    }

    private bool CanOpenDoor()
    {
        if (isLocked)
        {
            Debug.Log(lockedMessage);
            return false;
        }

        if (requiresAllItems)
        {
            PlayerObjectiveInventory inventory = FindObjectOfType<PlayerObjectiveInventory>();

            if (inventory == null || !inventory.HasAllItems())
            {
                Debug.Log(missingItemsMessage);
                return false;
            }
        }

        return true;
    }

    private IEnumerator RotateDoor(Quaternion targetRotation, bool opening)
    {
        isMoving = true;

        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / openDuration;

            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        transform.rotation = targetRotation;
        isOpen = opening;
        isMoving = false;

        if (isOpen)
        {
            Debug.Log(openMessage);

            if (disableColliderWhenOpen && doorCollider != null)
            {
                doorCollider.enabled = false;
            }
        }
        else
        {
            Debug.Log(closeMessage);

            if (doorCollider != null)
            {
                doorCollider.enabled = true;
            }
        }
    }
}
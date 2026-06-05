using UnityEngine;

public class AnimatedDoorInteractable : InteractableObject
{
    [Header("Animator")]
    public Animator doorAnimator;
    public string openTriggerName = "Open";
    public string closeTriggerName = "Close";

    [Header("Door State")]
    public bool isLocked = false;
    public bool requiresAllItems = false;
    public bool canClose = true;

    [Header("Messages")]
    public string lockedMessage = "La puerta está cerrada.";
    public string missingItemsMessage = "Parece que todavía falta algo...";
    public string openMessage = "La puerta se abre.";
    public string closeMessage = "La puerta se cierra.";

    [Header("Timing")]
    public float animationDuration = 1f;

    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
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
                CloseDoor();
            }

            return;
        }

        if (!CanOpenDoor())
        {
            return;
        }

        OpenDoor();
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

    private void OpenDoor()
    {
        isOpen = true;
        isMoving = true;

        Debug.Log(openMessage);

        if (doorAnimator != null)
        {
            doorAnimator.ResetTrigger(closeTriggerName);
            doorAnimator.SetTrigger(openTriggerName);
        }

        Invoke(nameof(StopMoving), animationDuration);
    }

    private void CloseDoor()
    {
        isOpen = false;
        isMoving = true;

        Debug.Log(closeMessage);

        if (doorAnimator != null)
        {
            doorAnimator.ResetTrigger(openTriggerName);
            doorAnimator.SetTrigger(closeTriggerName);
        }

        Invoke(nameof(StopMoving), animationDuration);
    }

    private void StopMoving()
    {
        isMoving = false;
    }
}
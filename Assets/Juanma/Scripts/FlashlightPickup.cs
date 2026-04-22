using UnityEngine;

public class FlashlightPickup : InteractableObject
{
    public override void Interact()
    {
        PlayerFlashlightController flashlightController = FindObjectOfType<PlayerFlashlightController>();

        if (flashlightController != null)
        {
            flashlightController.PickUpFlashlight();
            Destroy(gameObject);
        }
    }
}
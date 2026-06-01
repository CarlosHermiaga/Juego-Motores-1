using UnityEngine;

public class CollectibleItem : InteractableObject
{
    public string itemName = "objeto";

    public override void Interact()
    {
        PlayerObjectiveInventory inventory = FindObjectOfType<PlayerObjectiveInventory>();

        if (inventory != null)
        {
            inventory.AddItem();
            Debug.Log("Agarraste: " + itemName);
            Destroy(gameObject);
        }
    }
}
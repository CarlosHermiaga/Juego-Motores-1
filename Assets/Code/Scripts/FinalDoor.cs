using UnityEngine;

public class FinalDoor : InteractableObject
{
    public GameObject doorObject;

    public override void Interact()
    {
        PlayerObjectiveInventory inventory = FindObjectOfType<PlayerObjectiveInventory>();

        if (inventory == null) return;

        if (inventory.HasAllItems())
        {
            Debug.Log("La puerta final se abrió");

            if (doorObject != null)
            {
                doorObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Todavía faltan objetos para escapar");
        }
    }
}
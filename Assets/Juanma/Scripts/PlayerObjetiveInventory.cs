using UnityEngine;

public class PlayerObjectiveInventory : MonoBehaviour
{
    public int collectedItems = 0;
    public int requiredItems = 3;

    public void AddItem()
    {
        collectedItems++;
        Debug.Log("Objetos recolectados: " + collectedItems + "/" + requiredItems);
    }

    public bool HasAllItems()
    {
        return collectedItems >= requiredItems;
    }
}
using UnityEngine;
using TMPro;

public class PlayerObjectiveInventory : MonoBehaviour
{
    [Header("Objetivos")]
    public int collectedItems = 0;
    public int requiredItems = 3;

    [Header("UI")]
    public TextMeshProUGUI objectiveText;

    private void Start()
    {
        UpdateUI();
    }

    public void AddItem()
    {
        collectedItems++;

        if (collectedItems > requiredItems)
        {
            collectedItems = requiredItems;
        }

        UpdateUI();

        Debug.Log("Objetos recolectados: " + collectedItems + "/" + requiredItems);
    }

    public bool HasAllItems()
    {
        return collectedItems >= requiredItems;
    }

    private void UpdateUI()
    {
        if (objectiveText != null)
        {
            objectiveText.text = "Recuerdos encontrados: " + collectedItems + "/" + requiredItems;
        }
    }
}
using UnityEngine;
using TMPro;

public class PlayerObjectiveInventory : MonoBehaviour
{
    [Header("Objetivos")]
    public int collectedItems = 0;
    public int requiredItems = 3;

    [Header("Cassettes")]
    public int totalCassettes = 3;

    [Header("UI")]
    public TextMeshProUGUI objectiveText;
    public string objectiveLabel = "Cassettes encontrados";

    private bool[] collectedCassettes;
    private bool[] playedCassettes;

    private void Awake()
    {
        collectedCassettes = new bool[totalCassettes];
        playedCassettes = new bool[totalCassettes];
    }

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

    public void CollectCassette(int cassetteIndex)
    {
        if (!IsValidCassetteIndex(cassetteIndex))
        {
            Debug.LogWarning("Índice de cassette inválido: " + cassetteIndex);
            return;
        }

        if (collectedCassettes[cassetteIndex])
        {
            Debug.Log("Ese cassette ya fue recolectado.");
            return;
        }

        collectedCassettes[cassetteIndex] = true;

        AddItem();

        Debug.Log("Cassette encontrado: " + (cassetteIndex + 1) + "/" + totalCassettes);
    }

    public bool HasCassette(int cassetteIndex)
    {
        if (!IsValidCassetteIndex(cassetteIndex))
        {
            return false;
        }

        return collectedCassettes[cassetteIndex];
    }

    public bool HasAnyCassette()
    {
        for (int i = 0; i < collectedCassettes.Length; i++)
        {
            if (collectedCassettes[i])
            {
                return true;
            }
        }

        return false;
    }

    public int GetNextUnplayedCassetteIndex()
    {
        for (int i = 0; i < totalCassettes; i++)
        {
            if (collectedCassettes[i] && !playedCassettes[i])
            {
                return i;
            }
        }

        return -1;
    }

    public int GetFirstCollectedCassetteIndex()
    {
        for (int i = 0; i < totalCassettes; i++)
        {
            if (collectedCassettes[i])
            {
                return i;
            }
        }

        return -1;
    }

    public void MarkCassetteAsPlayed(int cassetteIndex)
    {
        if (!IsValidCassetteIndex(cassetteIndex))
        {
            return;
        }

        playedCassettes[cassetteIndex] = true;

        Debug.Log("Cassette reproducido: " + (cassetteIndex + 1));
    }

    private bool IsValidCassetteIndex(int cassetteIndex)
    {
        return cassetteIndex >= 0 && cassetteIndex < totalCassettes;
    }

    private void UpdateUI()
    {
        if (objectiveText != null)
        {
            objectiveText.text = objectiveLabel + ": " + collectedItems + "/" + requiredItems;
        }
    }
}
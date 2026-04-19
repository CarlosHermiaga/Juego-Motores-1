using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string interactionName = "objeto";

    public virtual void Interact()
    {
        Debug.Log("Interactuaste con " + gameObject.name);
    }
}
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    public TextMeshProUGUI interactionText;

    private InteractableObject currentInteractable;

    void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            InteractableObject interactable = hit.collider.GetComponentInParent<InteractableObject>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactionText.text = "Presioná E para interactuar con " + interactable.interactionName;
                return;
            }
        }

        currentInteractable = null;
        interactionText.text = "";
    }
}

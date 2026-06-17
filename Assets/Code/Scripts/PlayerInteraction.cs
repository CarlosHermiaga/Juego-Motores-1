using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Interaction Settings")]
    public float interactDistance = 3.5f;
    public float interactAngle = 35f;
    public LayerMask interactionMask = ~0;

    [Header("UI")]
    public TextMeshProUGUI interactionText;

    [Header("Debug")]
    public bool showDebug = false;

    private InteractableObject currentInteractable;

    private void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void CheckInteraction()
    {
        currentInteractable = null;

        if (playerCamera == null)
        {
            ClearInteractionText();
            return;
        }

        Collider[] hits = Physics.OverlapSphere(
            playerCamera.transform.position,
            interactDistance,
            interactionMask,
            QueryTriggerInteraction.Collide
        );

        InteractableObject bestInteractable = null;
        float bestScore = -999f;

        foreach (Collider hit in hits)
        {
            InteractableObject interactable = hit.GetComponentInParent<InteractableObject>();

            if (interactable == null)
            {
                continue;
            }

            Vector3 targetPoint = hit.bounds.center;
            Vector3 directionToTarget = targetPoint - playerCamera.transform.position;

            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget > interactDistance)
            {
                continue;
            }

            float angleToTarget = Vector3.Angle(playerCamera.transform.forward, directionToTarget);

            if (angleToTarget > interactAngle)
            {
                continue;
            }

            float angleScore = 1f - (angleToTarget / interactAngle);
            float distanceScore = 1f - (distanceToTarget / interactDistance);
            float finalScore = angleScore * 2f + distanceScore;

            if (finalScore > bestScore)
            {
                bestScore = finalScore;
                bestInteractable = interactable;
            }
        }

        if (bestInteractable != null)
        {
            currentInteractable = bestInteractable;

            if (interactionText != null)
            {
                interactionText.text = "Presioná E para " + currentInteractable.interactionName;
            }

            if (showDebug)
            {
                Debug.Log("Interactuable detectado: " + currentInteractable.gameObject.name);
            }

            return;
        }

        ClearInteractionText();
    }

    private void ClearInteractionText()
    {
        if (interactionText != null)
        {
            interactionText.text = "";
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerCamera.transform.position, interactDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance);
    }
}
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public static LoopManager Instance;

    [Header("Loop")]
    public Transform loopStartPoint;
    public Transform loop3StartPoint;
    public int currentLoop = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdvanceLoop(GameObject player)
    {
        currentLoop++;
        Debug.Log("Loop actual: " + currentLoop);

        Transform targetStartPoint = GetStartPointForCurrentLoop();

        if (targetStartPoint == null)
        {
            Debug.LogWarning("No hay StartPoint asignado para este loop.");
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = targetStartPoint.position;
            player.transform.rotation = targetStartPoint.rotation;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = targetStartPoint.position;
            player.transform.rotation = targetStartPoint.rotation;
        }
    }

    private Transform GetStartPointForCurrentLoop()
    {
        if (currentLoop >= 2 && loop3StartPoint != null)
        {
            return loop3StartPoint;
        }

        return loopStartPoint;
    }
}
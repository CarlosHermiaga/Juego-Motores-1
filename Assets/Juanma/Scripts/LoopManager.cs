using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public static LoopManager Instance;

    [Header("Loop")]
    public Transform loopStartPoint;
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

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = loopStartPoint.position;
            player.transform.rotation = loopStartPoint.rotation;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = loopStartPoint.position;
            player.transform.rotation = loopStartPoint.rotation;
        }
    }
}

using UnityEngine;

public class LoopStateGroup : MonoBehaviour
{
    [Header("Loop de activacion")]
    public int loopToActivate = 1;

    [Header("Objetos a activar")]
    public GameObject[] objectsToActivate;

    [Header("Objetos a desactivar")]
    public GameObject[] objectsToDeactivate;

    private bool hasChanged = false;

    void Update()
    {
        if (hasChanged) return;
        if (LoopManager.Instance == null) return;

        if (LoopManager.Instance.currentLoop >= loopToActivate)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            foreach (GameObject obj in objectsToDeactivate)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            hasChanged = true;
        }
    }
}
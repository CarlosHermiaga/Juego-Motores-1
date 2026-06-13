using UnityEngine;

public class LoopObjectActivator : MonoBehaviour
{
    [Header("Configuracion del loop")]
    public int loopToActivate = 1;

    [Header("Objetos")]
    public GameObject objectToActivate;
    public GameObject objectToDeactivate;

    private bool hasChanged = false;

    void Update()
    {
        if (hasChanged) return;
        if (LoopManager.Instance == null) return;

        if (LoopManager.Instance.currentLoop >= loopToActivate)
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }

            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false);
            }

            hasChanged = true;
        }
    }
}
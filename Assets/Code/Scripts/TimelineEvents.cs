using UnityEngine;

public class TimelineEvents : MonoBehaviour
{
    [SerializeField] GameObject[] objetosADesactivar;
    [SerializeField] GameObject[] objetosAActivar;

    public void FinTimeline()
    {
        foreach (GameObject obj in objetosADesactivar)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objetosAActivar)
        {
            obj.SetActive(true);
        }
    }
}

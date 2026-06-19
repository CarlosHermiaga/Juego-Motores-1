using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
   public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

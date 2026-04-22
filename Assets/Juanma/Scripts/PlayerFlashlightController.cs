using UnityEngine;

public class PlayerFlashlightController : MonoBehaviour
{
    public GameObject flashlightObject;
    public bool hasFlashlight = false;
    public KeyCode toggleKey = KeyCode.F;

    private bool isOn = false;

    void Start()
    {
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(false);
        }
    }

    void Update()
    {
        if (hasFlashlight && Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    public void PickUpFlashlight()
    {
        hasFlashlight = true;
        isOn = true;

        if (flashlightObject != null)
        {
            flashlightObject.SetActive(true);
        }

        Debug.Log("Agarraste la linterna");
    }

    public void ToggleFlashlight()
    {
        isOn = !isOn;

        if (flashlightObject != null)
        {
            flashlightObject.SetActive(isOn);
        }
    }
}

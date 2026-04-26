using UnityEngine;

public class PlayerFlashlightController : MonoBehaviour
{
    [SerializeField] GameObject flashlight, flashlightObject;
    bool hasFlashlight = false;
    [SerializeField] KeyCode toggleKey = KeyCode.F;

    private bool isOn = false;

    void Start()
    {
        if (flashlightObject != null)
        {
            flashlight.SetActive(false);
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
        //isOn = true;

        if (flashlightObject != null)
        {
            flashlight.SetActive(true);
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

using UnityEngine;

public class Radio : InteractableObject
{
    public bool isOn = false;

    public override void Interact()
    {
        isOn = !isOn;

        if (isOn)
        {
            Debug.Log("La radio se prendió");
        }
        else
        {
            Debug.Log("La radio se apagó");
        }
    }
}
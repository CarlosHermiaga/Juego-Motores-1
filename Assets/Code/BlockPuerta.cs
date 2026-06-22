using UnityEngine;

public class BlockPuerta : MonoBehaviour
{
    //private DoorSystem doorSystem;
    [SerializeField] DoorSystem doorSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    doorSystem = GetComponentInParent<DoorSystem>();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (doorSystem.doorOpen) 
        {
            doorSystem.doorOpen = false;
        }

        if (doorSystem.isLocked == false)
        {
            doorSystem.isLocked = true;
        }
    }
}

using System;
using UnityEngine;

public class ToggleMesh : MonoBehaviour
{
    [SerializeField] GameObject blockRoom;
    [SerializeField] GameObject doorRoom;
    [SerializeField] GameObject blockBath;
    [SerializeField] GameObject doorBath;
    [SerializeField] GameObject blockLiving;
    [SerializeField] GameObject doorLiving;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            blockRoom.SetActive(!blockRoom.activeSelf);
            doorRoom.SetActive(!doorRoom.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            blockBath.SetActive(!blockBath.activeSelf);
            doorBath.SetActive(!doorBath.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            blockLiving.SetActive(!blockLiving.activeSelf);
            doorLiving.SetActive(!doorLiving.activeSelf);
        }
    }
}

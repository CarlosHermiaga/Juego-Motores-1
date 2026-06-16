using UnityEngine;

public class Selected : MonoBehaviour
{

    [SerializeField] float distancia = 5f;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia))
        {
            if (hit.collider.tag == "Puerta")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.transform.GetComponent<DoorSystem>().ChangeDoorState();
                }

            }
        }  
    }
}

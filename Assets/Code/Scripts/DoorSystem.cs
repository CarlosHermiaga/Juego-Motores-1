using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [SerializeField] bool doorOpen = false;
    [SerializeField] float doorOpenAngle, doorCloseAngle, smooth;

    [SerializeField] AudioClip openDoor, closeDoor;

    public void ChangeDoorState()
    {
        doorOpen = !doorOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpen) 
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation2 = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Puerta")
        {
            AudioSource.PlayClipAtPoint(closeDoor, transform.position, 1);
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Puerta")
        {
            AudioSource.PlayClipAtPoint(openDoor, transform.position, 1);
        }
    }       
    
}

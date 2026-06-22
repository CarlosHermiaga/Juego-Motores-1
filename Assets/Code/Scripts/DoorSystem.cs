using System.Collections;
using UnityEngine;
using TMPro;

public class DoorSystem : MonoBehaviour
{
    [Header("Door State")]
    public bool doorOpen = false;
    [SerializeField] bool startsLocked = false;

    [Header("Door Rotation")]
    [SerializeField] float doorOpenAngle;
    [SerializeField] float doorCloseAngle;
    [SerializeField] float smooth = 5f;

    [Header("Door Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip openDoor;
    [SerializeField] AudioClip closeDoor;
    [SerializeField] AudioClip lockedDoor;
    [SerializeField] AudioClip unlockDoor;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI doorMessageText;
    [SerializeField] string lockedMessage = "La puerta está trabada.";
    [SerializeField] string unlockedMessage = "La puerta se destrabó.";
    [SerializeField] float messageDuration = 2f;
    [SerializeField] bool showUnlockedMessage = true;

    public bool isLocked;
    private Coroutine messageCoroutine;

    private void Awake()
    {
        isLocked = startsLocked;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        ClearMessage();
    }

    public void ChangeDoorState()
    {
        if (isLocked)
        {
            PlaySound(lockedDoor);
            ShowMessage(lockedMessage);

            Debug.Log("La puerta está trabada.");
            return;
        }

        doorOpen = !doorOpen;

        if (doorOpen)
        {
            PlaySound(openDoor);
            Debug.Log("Puerta abierta.");
        }
        else
        {
            PlaySound(closeDoor);
            Debug.Log("Puerta cerrada.");
        }
    }

    public void UnlockDoor()
    {
        if (!isLocked)
        {
            return;
        }

        isLocked = false;

        PlaySound(unlockDoor);

        if (showUnlockedMessage)
        {
            ShowMessage(unlockedMessage);
        }

        Debug.Log("Puerta destrabada.");
    }

    public void LockDoor()
    {
        isLocked = true;
        doorOpen = false;

        Debug.Log("Puerta trabada.");
    }

    private void Update()
    {
        if (doorOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, 1f);
        }
    }

    private void ShowMessage(string message)
    {
        if (doorMessageText == null)
        {
            return;
        }

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        doorMessageText.text = message;

        yield return new WaitForSeconds(messageDuration);

        ClearMessage();
    }

    private void ClearMessage()
    {
        if (doorMessageText != null)
        {
            doorMessageText.text = "";
        }
    }
}
using UnityEngine;

public class InfiniteEffects : MonoBehaviour
{
    bool shiftDown;
    [SerializeField] Transform player;
    [SerializeField] Camera cam;
    [SerializeField] float baseFov, maxFov, changeSpeed = 5f;
    float currentFov;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentFov = baseFov;
        cam.fieldOfView = baseFov;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            shiftDown = true;
        else
            shiftDown = false;

        if (shiftDown)
        {
            cam.fieldOfView = Mathf.Lerp(
            cam.fieldOfView,
            maxFov,
            Time.deltaTime * changeSpeed
            );
        }
        else 
        {
            cam.fieldOfView = Mathf.Lerp(
            cam.fieldOfView,
            currentFov,
            Time.deltaTime * changeSpeed
            );
        }
    }
}

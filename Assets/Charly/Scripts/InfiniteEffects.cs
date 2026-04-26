using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InfiniteEffects : MonoBehaviour
{
    bool inHallway;
    bool shiftDown;

    [SerializeField] Camera cam;
    [SerializeField] float baseFov, maxFov,changeSpeed = 5f;
    float currentFov;

    [SerializeField] Volume volume;
    Vignette vignette;
    ChromaticAberration chromatic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentFov = baseFov;
        cam.fieldOfView = baseFov;

        //volume.profile.TryGet(out vignette);
        //volume.profile.TryGet(out chromatic);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inHallway) 
        {
            ResetEffects();
            return;
        }

        //bool shift = Input.GetKey(KeyCode.LeftShift);

        //float targetFov = shift ? baseFov : maxFov;

        //cam.fieldOfView = Mathf.Lerp(
        //    cam.fieldOfView,
        //    targetFov,
        //    Time.deltaTime * changeSpeed
        //    );

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

    //void UpdatePostFX(bool running)
    //{
    //    float chromaTarget = running ? 1f : 0f;
    //    chromatic.intensity.value = Mathf.Lerp(
    //        chromatic.intensity.value,
    //        chromaTarget,
    //        Time.deltaTime * changeSpeed
    //        );

    //    float vignetteTarget = running ? 0.45f : 0.2f;
    //    float pulse = Mathf.Sin(Time.time * 4f) * 0.05f;
    //    vignette.intensity.value = Mathf.Lerp(
    //        vignette.intensity.value,
    //        vignetteTarget + pulse,
    //        Time.deltaTime * changeSpeed
    //    );
    //}

    void ResetEffects()
    {
        cam.fieldOfView = Mathf.Lerp(
                    cam.fieldOfView,
                    baseFov,
                    Time.deltaTime * changeSpeed
                );

        chromatic.intensity.value = Mathf.Lerp(
            chromatic.intensity.value,
            0f,
            Time.deltaTime * changeSpeed
        );

        vignette.intensity.value = Mathf.Lerp(
            vignette.intensity.value,
            0.2f,
            Time.deltaTime * changeSpeed
        );
    }

    public void SetInHallway(bool value)
    {
        inHallway = value;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!inHallway) return;
    }
}

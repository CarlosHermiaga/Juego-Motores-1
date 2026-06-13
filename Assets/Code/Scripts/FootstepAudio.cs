using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip walkClip;
    public AudioClip runClip;

    [Header("Intervalos")]
    public float walkStepInterval = 0.55f;
    public float runStepInterval = 0.32f;
    public float minSpeed = 0.1f;
    public float runSpeedThreshold = 7f;

    private AudioSource audioSource;
    private CharacterController cc;
    private float stepTimer;
    private Vector3 lastPosition;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        lastPosition = transform.position;

        // Crea su propio AudioSource por código, separado del de la lluvia
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
        audioSource.volume = 1f;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 delta = currentPos - lastPosition;
        delta.y = 0f;
        float speed = delta.magnitude / Time.deltaTime;
        lastPosition = currentPos;

        bool isMoving = speed > minSpeed && cc.isGrounded;

        if (isMoving)
        {
            bool isRunning = speed >= runSpeedThreshold;
            float interval = isRunning ? runStepInterval : walkStepInterval;

            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                AudioClip clip = isRunning ? runClip : walkClip;
                if (clip != null)
                    audioSource.PlayOneShot(clip);
                stepTimer = interval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}
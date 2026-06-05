using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemigoFisico : MonoBehaviour
{
    public Transform objetivo;
    public float velocidad = 3f;
    public float distanciaMinima = 1.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (objetivo == null) return;

        Vector3 direccion = objetivo.position - transform.position;
        direccion.y = 0f;

        float distancia = direccion.magnitude;

        if (distancia <= distanciaMinima)
            return;

        direccion.Normalize();

        rb.MovePosition(
            rb.position +
            direccion * velocidad * Time.fixedDeltaTime
        );

        transform.rotation = Quaternion.LookRotation(direccion);
    }
}
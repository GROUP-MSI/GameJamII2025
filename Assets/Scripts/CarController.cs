using UnityEngine;

public class CarController : MonoBehaviour
{
    // Variables de velocidad y control del auto
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;

    // Componente Rigidbody para controlar la física
    private Rigidbody rb;

    void Start()
    {
        // Obtener el Rigidbody del auto para aplicar fuerzas físicas
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Obtener la entrada del jugador para mover el auto
        float moveInput = Input.GetAxis("Vertical");  // W/S o flechas arriba/abajo
        float turnInput = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha

        // Llamar a los métodos para mover el auto
        MoveCar(moveInput);
        TurnCar(turnInput);
    }

    // Función para mover el auto hacia adelante o hacia atrás
    void MoveCar(float input)
    {
        // Aplicamos una fuerza hacia adelante (o atrás) dependiendo de la entrada del jugador
        Vector3 moveDirection = transform.forward * input * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    // Función para girar el auto
    void TurnCar(float input)
    {
        // Rotar el auto según la entrada horizontal (izquierda o derecha)
        float turnAmount = input * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}

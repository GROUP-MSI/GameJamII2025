using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxVelocityChange = 10f;
    
    [Header("Camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    public bool invertCamera = false;
    
    [Header("Jump")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    
    private Rigidbody rb;
    private float pitch = 0f;
    private float yaw = 0f;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotación de cámara
        RotateCamera();
        
        // Salto
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
        
        // Verificar si está en el suelo
        CheckGround();
    }

    void FixedUpdate()
    {
        // Movimiento del jugador
        MovePlayer();
    }

    void RotateCamera()
    {
        // Rotación horizontal (yaw)
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        
        // Rotación vertical (pitch)
        if (invertCamera)
            pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;
        else
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Limitar ángulo vertical
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        
        // Aplicar rotaciones
        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    void MovePlayer()
    {
        // Obtener input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        // Calcular velocidad objetivo
        Vector3 targetVelocity = transform.TransformDirection(input) * moveSpeed;
        
        // Calcular cambio de velocidad
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = targetVelocity - velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        
        // Aplicar fuerza
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    
    void CheckGround()
    {
        Vector3 origin = transform.position - new Vector3(0, transform.localScale.y * 0.5f, 0);
        float distance = 0.75f;
        
        isGrounded = Physics.Raycast(origin, Vector3.down, distance);
    }
}
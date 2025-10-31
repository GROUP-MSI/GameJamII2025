using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float sprintSpeed = 10f;
    public float jumpForce = 12f;
    public float groundDrag = 5f;
    public float airMultiplier = 0.4f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundLayer;
    
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    // Private variables
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool isGrounded;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Ground check - raycast hacia abajo
        // isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);

        // Obtener input del jugador
        GetInput();
        
        // Controlar velocidad
        SpeedControl();

        // Aplicar drag
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        // Mover el jugador
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Salto
        if (Input.GetKey(jumpKey) && isGrounded)
        {
            Jump();
        }

        // Sprint
        if (Input.GetKey(sprintKey) && isGrounded)
            currentSpeed = sprintSpeed;
        else
            currentSpeed = moveSpeed;
    }

    private void MovePlayer()
    {
        // Calcular dirección de movimiento basada en la orientación del jugador
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Movimiento en el suelo
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
        
        // Movimiento en el aire (con menor control)
        else
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limitar velocidad si es necesario
        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset velocidad en Y antes de saltar
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
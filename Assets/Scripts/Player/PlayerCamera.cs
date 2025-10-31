using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float sensX = 100f;
    public float sensY = 100f;
    public Transform cameraPosition;

    [Header("References")]
    public Transform playerBody;

    private float xRotation;
    private float yRotation;

    void Start()
    {
        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        transform.position = cameraPosition.position;
        // Obtener input del mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        // Limitar rotación vertical (para no dar vueltas completas)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotar cámara y jugador
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
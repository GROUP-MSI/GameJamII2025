using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    
    public Transform player;
    public Transform cameraPivot;

    public Vector3 baseOffset = new Vector3(0, 2, -3);
    public float minDistance = 1.5f;
    public float maxDistance = 4f;
    public float scrollSpeed = 2f;

    public float rotationSpeedH = 3f;
    public float rotationSpeedV = 3f;
    public float smoothTimePosition = 0.1f;
    public float smoothTimeRotation = 5f;

    private float yaw = 0f;
    private float pitch = 10f;
    private float currentDistance;

    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentDistance = -baseOffset.z;
        yaw = cameraPivot.localEulerAngles.y;
        pitch = cameraPivot.localEulerAngles.x;
    }

    void LateUpdate()
    {
        // Ajustar distancia con rueda del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance - scroll * scrollSpeed, minDistance, maxDistance);

        // Escalar offset según la escala del jugador (asumiendo escala uniforme)
        float scaleFactor = player.localScale.x;
        Vector3 scaledOffset = baseOffset * scaleFactor;
        scaledOffset.z = -currentDistance * scaleFactor;

        // Limitar sensibilidad según distancia para evitar giros bruscos cerca del jugador
        float distanceFactor = Mathf.InverseLerp(minDistance, maxDistance, currentDistance);
        float adjustedRotationSpeedH = rotationSpeedH * distanceFactor;
        float adjustedRotationSpeedV = rotationSpeedV * distanceFactor;

        // Rotar el pivot con el mouse
        yaw += adjustedRotationSpeedH * Input.GetAxis("Mouse X");
        pitch -= adjustedRotationSpeedV * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -35f, 60f);

        // Mantener yaw en rango 0-360 para evitar overflow
        if (yaw < 0) yaw += 360f;
        else if (yaw > 360f) yaw -= 360f;

        cameraPivot.localRotation = Quaternion.Euler(pitch, yaw, 0);

        // Evitar que la cámara atraviese obstáculos
        Vector3 desiredCameraDir = cameraPivot.rotation * scaledOffset.normalized;
        float desiredDistance = scaledOffset.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(cameraPivot.position, desiredCameraDir, out hit, desiredDistance))
        {
            float adjustedDistance = hit.distance * 0.9f; // un poco antes del obstáculo
            scaledOffset = scaledOffset.normalized * adjustedDistance;
        }

        // Posición objetivo de la cámara: posición del pivot + offset rotado y ajustado
        Vector3 targetPosition = cameraPivot.position + scaledOffset;

        // Suavizado de posición para que la cámara siga suavemente
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTimePosition);

        // Rotar la cámara para mirar al pivot (jugador)
        Quaternion targetRotation = Quaternion.LookRotation(cameraPivot.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTimeRotation * Time.deltaTime);
    }
}

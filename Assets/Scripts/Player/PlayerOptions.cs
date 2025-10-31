using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptions : MonoBehaviour
{
    [Header("Configuración general")]
    public Light playerLamp;
    public bool lampEnabledAtStart = false;

    [Header("Atajos de teclado")]
    public KeyCode toggleLampKey = KeyCode.F;
    public KeyCode toggleMuteKey = KeyCode.M;
    public KeyCode openMenuKey = KeyCode.Escape;

    [Header("Opciones de jugador")]
    public bool isMuted = false;
    public bool menuOpen = false;

    void Start()
    {
        // Configurar la linterna según el estado inicial
        if (playerLamp != null)
            playerLamp.enabled = lampEnabledAtStart;
    }

    void Update()
    {
        HandleShortcuts();
    }

    private void HandleShortcuts()
    {
        // Encender / apagar linterna
        if (Input.GetKeyDown(toggleLampKey))
            ToggleLamp();

        // Silenciar / activar sonido
        if (Input.GetKeyDown(toggleMuteKey))
            ToggleMute();

        // Abrir / cerrar menú de opciones
        if (Input.GetKeyDown(openMenuKey))
            ToggleMenu();
    }

    private void ToggleLamp()
    {
        if (playerLamp == null) return;

        playerLamp.enabled = !playerLamp.enabled;
        Debug.Log($"Linterna {(playerLamp.enabled ? "encendida" : "apagada")}");
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
        Debug.Log($"Sonido {(isMuted ? "silenciado" : "activado")}");
    }

    private void ToggleMenu()
    {
        menuOpen = !menuOpen;
        Debug.Log($"Menú {(menuOpen ? "abierto" : "cerrado")}");
    }
}

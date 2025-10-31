using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerGame : MonoBehaviour
{
    [Header("UI Slider")]
    public Slider slider;
    public Image fillImage;

    [Header("Colores de salud")]
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;

    [Header("Salud del jugador")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Daño continuo del monstruo")]
    public float monsterDamagePerSecond = 5f; // daño gradual por segundo

    [Header("Objetos Active")]
    public GameObject btnAction;
    public GameObject btnActionFinal;
    public GameObject btnActionKey;

    public AudioSource home1;
    public AudioSource home2;

	private bool hasKey;

	public GameObject panelSucess;
	public GameObject linterna;
	public AudioSource car;

    private void Start()
	{
		linterna.SetActive(false);
		panelSucess.SetActive(false);
		btnAction.SetActive(false);
		btnActionKey.SetActive(false);
		btnActionFinal.SetActive(false);
        hasKey = false;
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
    }

    private void Update()
    {
		// Evita que la salud sea negativa
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		
		if (Input.GetMouseButton(1)) // botón derecho del mouse
		{
			linterna.SetActive(true);
		}
		else
		{
			linterna.SetActive(false);
		}
    }

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("monster"))
		{
			TakeDamage(monsterDamagePerSecond * Time.deltaTime);
		}

		if (other.CompareTag("Car") && hasKey)
			btnActionFinal.SetActive(true);

		if (other.CompareTag("key") && !hasKey)
		{
			btnActionKey.SetActive(true);

			if (Input.GetKeyDown(KeyCode.R))
			{
				hasKey = true;
				Destroy(other.gameObject);
				btnActionKey.SetActive(false);
			}
		}

		if (other.CompareTag("casa1") || other.CompareTag("Car") || other.CompareTag("casa2"))
			btnAction.SetActive(true);

		// 🏠 Interacción con casas
		if (other.CompareTag("casa1") && Input.GetKeyDown(KeyCode.E))
			home1.Play();

		if (other.CompareTag("casa2") && Input.GetKeyDown(KeyCode.E))
			home2.Play();


		if (other.CompareTag("Car") && Input.GetKeyDown(KeyCode.L) && hasKey)
		{
			panelSucess.SetActive(true);
			car.Play();
			StartCoroutine(ChangeSceneAfterDelay("Menu", 3f));
		}
	}
	
	private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(sceneName);
	}

    private void OnTriggerExit(Collider other)
    {
        // 🔄 Ocultar botones al salir del área
        btnAction.SetActive(false);
        btnActionKey.SetActive(false);
        btnActionFinal.SetActive(false);
    }

    private void TakeDamage(float amount)
    {
        currentHealth -= amount;
        SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("💀 El jugador ha muerto.");
    }

    public void SetMaxHealth(float health)
    {
        if (slider == null) return;

        slider.maxValue = health;
        slider.value = health;

        if (fillImage != null)
            fillImage.color = fullHealthColor;
    }

    public void SetHealth(float health)
    {
        if (slider == null) return;

        slider.value = health;

        if (fillImage != null)
            fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, health / slider.maxValue);
    }
}

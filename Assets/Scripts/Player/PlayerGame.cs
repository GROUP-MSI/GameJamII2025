using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayerGame : MonoBehaviour
{
	[Header("UI Slider")]
	public Slider slider;
	public Image fillImage;

	[Header("Colores")]
	public Color fullHealthColor = Color.green;
	public Color zeroHealthColor = Color.red;

	[Header("Salud del jugador")]
	public float maxHealth = 100f;
	private float currentHealth;

	[Header("Daños por tag")]
	public Dictionary<string, float> damageByTag = new Dictionary<string, float>()
	{
			{ "Enemy", 10f },
			{ "Trap", 20f },
			{ "Bullet", 15f }
	};

	[Header("Objetos Active")]
	public GameObject btnAction;
	public GameObject btnActionFinal;
	public GameObject btnActionKey;
	public GameObject panelText;
	public TMP_Text textMessageHome;

	private bool hasKey;

	private void Start()
	{
		slider.value = 1;
		hasKey = false;
		currentHealth = maxHealth;
		SetMaxHealth(maxHealth);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (damageByTag.TryGetValue(collision.gameObject.tag, out float damage))
		{
			TakeDamage(damage);
		}
	}

	private void TakeDamage(float amount)
	{
		currentHealth -= amount;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		SetHealth(currentHealth);

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		Debug.Log("Jugador ha muerto.");
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

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "car" && hasKey)
			btnActionFinal.SetActive(true);

		if (other.gameObject.tag == "key" && !hasKey)
			btnActionKey.SetActive(true);

		btnAction.SetActive(true);

		if (other.gameObject.tag == "casa1" && Input.GetKeyDown(KeyCode.E))
		{
			panelText.SetActive(true);
			textMessageHome.text = "ESTAS HEBRIO. YA ES HORA DEL UNCHANCHU, TE VA A LLEVAR EL DIABLO";
		}

		if (other.gameObject.tag == "casa2" && Input.GetKeyDown(KeyCode.E))
		{
			panelText.SetActive(true);
			textMessageHome.text = "¡FUERA, COMPADRE! ES EL ALTIPLANO, A ESTA HORA CAMINA EL UNCHANCHU";
		}


		if (other.gameObject.tag == "key" && Input.GetKeyDown(KeyCode.R))
		{
			panelText.SetActive(true);
			textMessageHome.text = "¡Fuera, compadre! En el altiplano, a esta hora camina el Unchanchu.";
		}

		if (other.gameObject.tag == "car" && Input.GetKeyDown(KeyCode.L))
		{

		}
		

	}


  void OnTriggerExit(Collider other)
  {
		panelText.SetActive(false);
    btnAction.SetActive(false);
    btnActionKey.SetActive(false);
    btnActionFinal.SetActive(false);
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

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
	}


  void OnTriggerExit(Collider other)
  {
    btnAction.SetActive(false);
    btnActionKey.SetActive(false);
    btnActionFinal.SetActive(false);
  }
}

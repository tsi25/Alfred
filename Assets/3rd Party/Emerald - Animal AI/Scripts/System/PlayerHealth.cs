using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour 
{
	public float startingHealth = 100;
	public float currentHealth = 100;
	public Text healthText = null; 
	float timer;

	void Awake ()
	{
		currentHealth = startingHealth;

		healthText = GameObject.Find("HealthText").GetComponent<Text>();
		healthText.text = currentHealth.ToString() + "%";
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (timer >= 1 && currentHealth < 100)
		{
			currentHealth += 1;
			timer = 0;
		}

		if (healthText != null)
		{
			healthText.text = currentHealth.ToString() + "%";
		}
	}

	public void DamagePlayer (float damageTaken)
	{
		currentHealth -= damageTaken;

		if (healthText != null)
		{
			//healthText.text = currentHealth.ToString() + "%";
		}

		if (currentHealth <= 0)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}

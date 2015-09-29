using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class PlayerWeapon : MonoBehaviour {
	
	public GameObject bloodEffect;
	public bool useBloodEffect = false;
	public GameObject hitOtherEffect;

	public string predatorTagName = "Predator";
	public string preyTagName = "Huntable";
	public string surfaceName1 = "Untagged";
	public string surfaceName2 = "Terrain";

	public float attackDistance = 4.0f;
	public float attackDelay = 0.7f;
	public float damage = 5;
	public float attackTime = 1;
	public AudioClip[] impactHitAnimalSounds;
	public AudioClip[] impactHitOtherSounds;
	
	private bool calculatedHit = false;
	private Ray ray;
	private RaycastHit hit;
	private float timer;
	private float attackTimer;
	private AudioSource audio_Source;

	void Start () 
	{
		audio_Source = GetComponent<AudioSource>();
	}

	void FixedUpdate () 
	{
		if(Input.GetMouseButton(0) && attackTimer >= attackTime && !calculatedHit)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit, attackDistance))
			{
				calculatedHit = true;
			}
		}
	}

	void Update ()
	{
		if (!calculatedHit)
		{
			attackTimer += Time.deltaTime;
		}


		if (calculatedHit)
		{
			timer += Time.deltaTime;

			if (hit.collider == null)
			{
				calculatedHit = false;
			}
			
			if (timer >= attackDelay)
			{
				if (hit.collider != null && hit.collider.tag == predatorTagName || hit.collider.tag == preyTagName && hit.collider != null)
				{
					if (hit.collider.GetType() == typeof(BoxCollider))
					{
						if (useBloodEffect)
						{
							Instantiate(bloodEffect, hit.point, Quaternion.identity);
						}

						audio_Source.PlayOneShot(impactHitAnimalSounds[Random.Range(0,impactHitAnimalSounds.Length)]);
						hit.collider.gameObject.GetComponent<Emerald_Animal_AI>().Damage(damage);
					}
				}

				//If the weapon hits another surface (ground, grass, wood, etc) use other effect
				if (hit.collider.tag == surfaceName1 || hit.collider.tag == surfaceName2)
				{
					Instantiate(hitOtherEffect, hit.point, Quaternion.LookRotation(hit.normal));
					audio_Source.PlayOneShot(impactHitOtherSounds[Random.Range(0,impactHitOtherSounds.Length)]);
				}

				attackTimer = 0; 
				calculatedHit = false;
				timer = 0;
			}
		}
	}
}

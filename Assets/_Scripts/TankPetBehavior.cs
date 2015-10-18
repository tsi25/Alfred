using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TankPetBehavior : MonoBehaviour 
{

	public float walkSpeed = 0.75f;
	public float runSpeed = 1.5f;

	public float waitMin = 1f;
	public float waitMax = 7f;

	public Animation animation;
	public SkinnedMeshRenderer petMeshRenderer;

	public Material defaultMaterial;
	public Material sleepMaterial;

	public ParticleSystem sleepParticle;
	public GameObject phone;

	public enum AnimState{idle, idle2, walk, run, jump, eat, sleep, wakeup, none};

	public List<Transform> roamingPoints = new List<Transform> ();


	private AnimState currentAnimState = AnimState.none;
	private Transform currentTransform;
	private float currentAnimationTime = 0f;

	private Vector3 desiredPos;
	private Transform desiredRotationTarget;

	public AnimState CurrentAnimState
	{
		get { return currentAnimState; }
		set
		{
			if(currentAnimState != value)
			{
				currentAnimState = value;
				
				switch(currentAnimState)
				{
				case AnimState.eat:
					currentAnimationTime = animation["Hurt"].length;
					animation.Play ("Hurt");
					break;
					
				case AnimState.walk:
					animation.Play ("Walk");
					break;
					
				case AnimState.run:
					animation.Play ("Run");
					break;

				case AnimState.jump:
					currentAnimationTime = animation["Attack"].length;
					animation.Play ("Attack");
					break;
					
				case AnimState.idle:
					animation.Play("Idle");
					break;

				case AnimState.idle2:
					animation.Play("Idle2");
					break;

				case AnimState.sleep:
					sleepParticle.Play();
					currentAnimationTime = animation["Move Die"].length;
					animation.Play("Move Die");
					break;

				case AnimState.wakeup:
					sleepParticle.Stop();
					currentAnimationTime = animation["Move Die"].length;
					animation.Rewind("Move Die");
					break;

				default:
					break;
				}
			}
		}
	}


	public void MoveTo(Transform t)
	{
		StopAllCoroutines();

		if(currentAnimState == AnimState.sleep) StartCoroutine (ForceWakeUp ());

		StartCoroutine(Roam(t));
	}


	public void StartInteraction (PetInteractibleObject pio)
	{
		StopAllCoroutines();

		if(CurrentAnimState == AnimState.sleep)
		{
			StartCoroutine(ForceWakeUp (pio));
			return;
		}

		StartCoroutine (Interact(pio));
	}
	

	private void OpenTheGate(string s)
	{
		StopAllCoroutines();

		switch(s)
		{
		case "Roam":
			StartCoroutine (Roam ());
			break;

		case "Wait":
			StartCoroutine (Wait ());
			break;

		case "Idle2":
			StartCoroutine (CheckPhone ());
			break;

		default:
			break;
		}
	}


	private IEnumerator ForceWakeUp(PetInteractibleObject pio = null)
	{
		CurrentAnimState = AnimState.wakeup;
		yield return new WaitForSeconds(animation["Move Die"].length);
		CurrentAnimState = AnimState.idle;

		if (pio != null) StartInteraction(pio);
	}


	private IEnumerator Roam(Transform t = null)
	{
		Transform target;
		//pick a point to roam to
		if(t != null)
		{
			target = t;
		}
		else
		{
			int randy = Mathf.RoundToInt(Random.Range(0, roamingPoints.Count));
			target = roamingPoints[randy];
		}

		if(currentTransform != null && target.position == currentTransform.position)
		{
			OpenTheGate("Roam");
		}

		Vector3 startPos = transform.position;

		//find how long it should take to get there
		float desiredTime = (Vector3.Distance(transform.position, target.position))/walkSpeed;
		float elapsedTime = 0f;

		//start moving
		CurrentAnimState = AnimState.walk;
		transform.LookAt(target);
		desiredRotationTarget = target;
		while(elapsedTime <= desiredTime)
		{
			transform.position = Vector3.Lerp(startPos, target.position, elapsedTime/desiredTime);
			desiredPos = Vector3.Lerp(startPos, target.position, elapsedTime/desiredTime);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		//finish moving
		CurrentAnimState = AnimState.idle;

		OpenTheGate("Wait");
	}


	private IEnumerator Wait()
	{
		CurrentAnimState = AnimState.idle;

		float waitTime = Random.Range(waitMin, waitMax);

		yield return new WaitForSeconds(waitTime);

		if(Random.Range(0, 10) > 4)
		{
			OpenTheGate("Idle2");
		}
		else
		{
			OpenTheGate("Roam");
		}

	}


	private IEnumerator CheckPhone()
	{
		phone.SetActive(true);
		CurrentAnimState = AnimState.idle2;

		yield return new WaitForSeconds(waitMax * 2);


		phone.SetActive(false);
		OpenTheGate("Roam");

	}


	private IEnumerator Interact(PetInteractibleObject pio)
	{
		//pick a point to roam to
		Vector3 startPos = transform.position;

		//find how long it should take to get there
		float desiredTime = (Vector3.Distance(transform.position, pio.StandPosition))/runSpeed;
		float elapsedTime = 0f;

		//start moving
		CurrentAnimState = AnimState.run;
		transform.LookAt(pio.StandPosition);
		desiredRotationTarget = pio.standPosition;

		while(elapsedTime < desiredTime)
		{
			transform.position = Vector3.Lerp(startPos, pio.StandPosition, elapsedTime/desiredTime);
			desiredPos = Vector3.Lerp(startPos, pio.StandPosition, elapsedTime/desiredTime);

			elapsedTime += Time.deltaTime;
			yield return null;
		}
		
		//finish moving
		transform.LookAt(pio.LookDirection);
		desiredRotationTarget = pio.LookDirection;
		CurrentAnimState = pio.myAnimState;

		//start interacting
		yield return new WaitForSeconds(currentAnimationTime);

		//check interaction
		if (CurrentAnimState == AnimState.sleep)
		{
			//sleep forever i guess - or until whatever?
			yield return new WaitForSeconds(60f);
			CurrentAnimState = AnimState.wakeup;
			yield return new WaitForSeconds(animation["Move Die"].length);
			CurrentAnimState = AnimState.idle;
		}
		else
		{
			CurrentAnimState = AnimState.idle;
		}


		OpenTheGate("Wait");
	}


	private void LateUpdate()
	{
		if(desiredPos != null) transform.position = desiredPos;
		if(desiredRotationTarget != null) transform.LookAt (desiredRotationTarget);
	}


	private IEnumerator RunOnWheel()
	{
		yield return null;
	}


	private void Start()
	{
		desiredPos = transform.position;
		CurrentAnimState = AnimState.idle;
		StartCoroutine(Wait());
	}
}

using UnityEngine;
using System.Collections;


public class Animate : MonoBehaviour 
{
	public GameObject ai;
	public Animation animation;
	public NavMeshAgent navMeshAgent;
	public Transform target;
	public Fetch fetch;

	public float runSpeed = 1.25f;
	public float walkSpeed = 0.75f;

	public enum AnimState{idle, walk, run, eat};

	private AnimState currentAnimState = AnimState.idle;
	private Vector3 posLastFrame;
	private bool moving = false;



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
					if(!animation.IsPlaying("Hurt")) animation.Play ("Hurt");
					break;

				case AnimState.walk:
					if(!animation.IsPlaying("Walk")) animation.Play ("Walk");
					break;

				case AnimState.run:
					if(!animation.IsPlaying("Run")) animation.Play ("Run");
					break;

				case AnimState.idle:
				default:
					if(!animation.IsPlaying("Idle")) animation.Play("Idle");
					break;
				}
			}
		}
	}


//	private void FixedUpdate()
//	{
//		Vector3 posThisFrame = ai.transform.position;
//
//		if(posThisFrame != posLastFrame)
//		{
//			moving = true;
//			float speed = Vector3.Distance(posThisFrame, posLastFrame);
//
//			if(speed <= 0.06f)
//			{
//				CurrentAnimState = AnimState.walk;
//			}
//			else
//			{
//				CurrentAnimState = AnimState.run;
//			}
//		}
//		else if (moving)
//		{
//			moving = false;
//			CurrentAnimState = AnimState.idle;
//		}
//
//		posLastFrame = posThisFrame;
//	}


	private void LateUpdate()
	{
		if(transform.position.y != -1.1791f)
		{
			transform.position = new Vector3 (transform.position.x, -1.1791f, transform.position.z);
		}
	}


	private void Start()
	{
		currentAnimState = AnimState.idle;
	}
}

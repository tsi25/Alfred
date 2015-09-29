using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fetch : MonoBehaviour 
{
	public FetchBall ball;
	public Transform defaultPosition;
	public Transform holdPosition;
	public Transform mouthPosition;

	public float throwForce = 5f;

	public Animate animate;

	public Image pickUpIcon;

	private bool isHoldingBall = false;
	private bool ballThrown = false;
	private bool lockedToMouth = false;

	private IEnumerator ThrowBall()
	{
		isHoldingBall = false;
		lockedToMouth = false;

		ball.transform.SetParent(defaultPosition);
		ball.rigidBody.velocity = transform.forward * throwForce;
		ball.rigidBody.useGravity = true;
		ballThrown = true;

		//ball.rigidBody.AddForce();

		yield return new WaitForSeconds(1f);

		StartCoroutine (Chase ());
	}


	private void PickupBall()
	{
		ball.rigidBody.useGravity = false;
		ball.transform.SetParent(holdPosition);
		ball.transform.position = Vector3.zero;
		ballThrown = false;
	}


	public IEnumerator Chase()
	{
		animate.navMeshAgent.speed = animate.runSpeed;
		animate.CurrentAnimState = Animate.AnimState.run;


		while(Vector3.Distance(animate.navMeshAgent.transform.position, ball.transform.position) > 0.7f)
		{
			animate.navMeshAgent.SetDestination(new Vector3(ball.transform.position.x,
			                                                0,
			                                                ball.transform.position.z));

			yield return null;
		}
		StopMoving();
		
		//animate.navMeshAgent.Stop();
		animate.CurrentAnimState = Animate.AnimState.eat;
		
		yield return new WaitForSeconds(0.51f);
		
		ball.transform.SetParent(mouthPosition);
		lockedToMouth = true;
		
		yield return new WaitForSeconds(0.51f);
		
		animate.CurrentAnimState = Animate.AnimState.idle;
		StartCoroutine (Return ());
	}


	public IEnumerator Return()
	{
		//animate.navMeshAgent.Stop ();
		animate.navMeshAgent.speed = animate.walkSpeed;
		animate.CurrentAnimState = Animate.AnimState.walk;

		while(Vector3.Distance(animate.navMeshAgent.transform.position, transform.position) > 1.4)
		{
			animate.navMeshAgent.SetDestination(new Vector3(transform.position.x,
			                                                0,
			                                                transform.position.z));

			yield return null;
		}
		StopMoving();

		ball.transform.SetParent(defaultPosition);
		lockedToMouth = false;
		animate.CurrentAnimState = Animate.AnimState.idle;
	}


	private void StopMoving()
	{
		Vector3 myPos = animate.navMeshAgent.transform.position;
		animate.navMeshAgent.SetDestination(new Vector3(myPos.x,
		                                                0,
		                                                myPos.z));
	}


	private void Update()
	{
		if(isHoldingBall && Input.GetMouseButtonDown(0))
		{
		    StartCoroutine(ThrowBall());
		}
		else if (isHoldingBall)
		{
			lockedToMouth = false;
			if(pickUpIcon.enabled) pickUpIcon.enabled = false;
			ball.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			ball.transform.position = holdPosition.position;
		}
		else
		{
			Debug.DrawLine(transform.position, transform.forward, Color.red);
			RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity);
			bool hovering = false;

			foreach(RaycastHit hit in hits)
			{
				if(hit.collider.gameObject.tag == "Ball")
				{
				    hovering = true;
					break;
				}

			    hovering = false;
			}

			pickUpIcon.enabled = hovering;

			if(hovering && Input.GetMouseButton(0))
			{
				PickupBall();
				isHoldingBall = true;
			}
		}

		if(lockedToMouth && !isHoldingBall)
		{
			ball.GetComponent<Rigidbody> ().useGravity = false;
			ball.transform.position = mouthPosition.position;
		}
		else
		{
			ball.GetComponent<Rigidbody> ().useGravity = true;
		}
	}
}

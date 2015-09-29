using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour 
{
	private Vector3 cachedPosition;

	void FixedUpdate () 
	{
		transform.rotation = Input.gyro.attitude;
		transform.Translate(Input.acceleration.x - cachedPosition.x,
		                    Input.acceleration.y - cachedPosition.y,
		                    Input.acceleration.z - cachedPosition.z);
	}
}

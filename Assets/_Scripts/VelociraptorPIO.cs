using UnityEngine;
using System.Collections;

public class VelociraptorPIO : PetInteractibleObject 
{
	public Transform velociraptor;
	public GameObject lookDirectionPrefab;

	public override Vector3 StandPosition
	{
		get 
		{ 
			return velociraptor.position; 
		}
	}

	public override Transform LookDirection
	{
		get 
		{ 
			Vector3 t = Camera.main.transform.position;
			GameObject go = Instantiate(lookDirectionPrefab, new Vector3(t.x, velociraptor.position.y, t.z), Quaternion.identity) as GameObject;
			return go.transform;
		}
	}
}

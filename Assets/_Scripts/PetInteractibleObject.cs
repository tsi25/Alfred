using UnityEngine;
using System.Collections;

public class PetInteractibleObject : MonoBehaviour 
{
	public Transform standPosition;
	public Transform lookDirection;


	public virtual Vector3 StandPosition
	{
		get{ return standPosition.position; }
	}

	public virtual Transform LookDirection
	{
		get{ return lookDirection; }
	}


	public TankPetBehavior.AnimState myAnimState;
}

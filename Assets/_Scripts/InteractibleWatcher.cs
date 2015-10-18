using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractibleWatcher : MonoBehaviour 
{
	public TankPetBehavior tankPetBehavior;

	public Color defaultColor;
	public Color disabledColor;

	public GameObject roamingPointPrefab;

	private bool tankPlaced;

	private void FixedUpdate()
	{
		if(Input.GetMouseButtonDown(0))
		{
			bool moveOnly = true;
			Ray origin;

			origin = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

			RaycastHit[] hits = Physics.RaycastAll(origin);

			foreach(RaycastHit hit in hits)
			{
				if(tankPetBehavior != null)
				{
					if(hit.collider.gameObject.tag == "Interactible")
					{
						Debug.Log ("wakka wakka");
						PetInteractibleObject pio = hit.collider.gameObject.GetComponent<PetInteractibleObject> ();
						tankPetBehavior.StartInteraction(pio);
						moveOnly = false;
						break;
					}
				}
			}

			if(moveOnly)
			{
				foreach(RaycastHit hit in hits)
				{
					if(hit.collider.gameObject.tag == "Ground")
					{
						GameObject t = Instantiate(roamingPointPrefab, hit.point, Quaternion.identity) as GameObject;
						tankPetBehavior.MoveTo(t.transform);
						break;
					}
				}
			}
		}
	}


	private IEnumerator SeekTPB()
	{
		while(tankPetBehavior == null)
		{
			tankPetBehavior = GameObject.FindObjectOfType<TankPetBehavior> ();
			yield return null;
		}
	}


	private void Start()
	{
		StartCoroutine(SeekTPB());
	}
}

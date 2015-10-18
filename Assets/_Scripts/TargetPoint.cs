using UnityEngine;
using System.Collections;

public class TargetPoint : MonoBehaviour 
{
	public ParticleSystem ps;

	private IEnumerator DestroyMyself()
	{
		yield return new WaitForSeconds(20f);
		Destroy(this.gameObject);
	}


	private void Start()
	{
		ps.Emit(1);
		StartCoroutine(DestroyMyself ());
	}
}

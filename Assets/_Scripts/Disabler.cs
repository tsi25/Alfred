using UnityEngine;
using System.Collections;

public class Disabler : MonoBehaviour 
{
	private IEnumerator DelayedShutdown()
	{
		yield return new WaitForSeconds(0.1f);
		gameObject.SetActive(false);
	}


	private void Update()
	{
		if(Input.touchCount == 1)
		{
			StartCoroutine(DelayedShutdown ());
		}
	}
}

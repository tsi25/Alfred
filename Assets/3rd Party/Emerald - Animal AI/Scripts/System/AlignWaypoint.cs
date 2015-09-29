using UnityEngine;
using System.Collections;

public class AlignWaypoint : MonoBehaviour 
{
	void Update () 
	{
		if (Terrain.activeTerrain == true)
		{
			float wayPointHeight = Terrain.activeTerrain.SampleHeight(transform.position);
			transform.position = new Vector3(transform.position.x, wayPointHeight, transform.position.z);
		}	
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public WebCamTexture mCamera = null;
	public GameObject plane;
	

	void Start ()
	{
		mCamera = new WebCamTexture ();
		mCamera.Play ();
		gameObject.GetComponent<Renderer> ().material.mainTexture = mCamera;
		if(!mCamera.isPlaying) mCamera.Play ();
	}
}
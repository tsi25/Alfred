using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud : MonoBehaviour 
{
	public Button quitButton;
	public Button resetButton;


	private void Quit()
	{
		Application.Quit ();
	}


	private void Reset()
	{
		Application.LoadLevel(Application.loadedLevel);
	}


	private void FixedUpdate()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) Quit ();
	}


	private void Start () 
	{
		quitButton.onClick.AddListener (Quit);
		resetButton.onClick.AddListener (Reset);
	}
}

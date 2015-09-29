//Emerald - Animal AI Menu by: Black Horizon Studios
//Version 1.0

using UnityEditor;
using UnityEngine;
public class Emerald_Animal_AI_Menu : MonoBehaviour 
{
    [MenuItem ("Window/Emerald - Animal AI/Create/Create Animal AI")]
    static void CreateAISystem () 
	{
		foreach (GameObject obj in Selection.gameObjects) 
		{
			obj.AddComponent<Emerald_Animal_AI>();
		}
    }

    [MenuItem ("Window/Emerald - Animal AI/Wiki/Home")]
    static void Home ()
    {
		Application.OpenURL("http://emerald-animal-ai.wikia.com/wiki/Emerald_Animal_AI_Wikia");
    }

	[MenuItem ("Window/Emerald - Animal AI/Wiki/Player Weapon and Health Setup")]
	static void PlayerWeaponSetup ()
	{
		Application.OpenURL("http://emerald-animal-ai.wikia.com/wiki/Tutorials#Setting_up_the_Player_Weapon_and_Health_System");
	}

	[MenuItem ("Window/Emerald - Animal AI/Wiki/Documentation")]
	static void Documentation ()
	{
		Application.OpenURL("http://emerald-animal-ai.wikia.com/wiki/Documentation");
	}
	
	[MenuItem ("Window/Emerald - Animal AI/Wiki/Introduction")]
    static void Introduction ()
    {
		Application.OpenURL("http://emerald-animal-ai.wikia.com/wiki/Introduction");
    }
	
	[MenuItem ("Window/Emerald - Animal AI/Wiki/Tutorials")]
    static void Tutorials ()
    {
		Application.OpenURL("http://emerald-animal-ai.wikia.com/wiki/Tutorials");
    }
	
	[MenuItem ("Window/Emerald - Animal AI/Wiki/Code References")]
    static void CodeReferences ()
    {
		Application.OpenURL("http://emerald-animal-ai.wikia.com/wiki/Code_References");
    }

	[MenuItem ("Window/Emerald - Animal AI/Wiki/Solutions to Possible Issues")]
	static void PossibleIssues ()
	{
		Application.OpenURL("http://emerald-animal-ai.wikia.com/wiki/Solutions_to_Possible_Issues");
	}

   
	[MenuItem ("Window/Emerald - Animal AI/Customer Service")]
    static void CustomerService ()
    {
		Application.OpenURL("http://blackhorizonstudios.webs.com/customersupport.htm");
    }
	
}
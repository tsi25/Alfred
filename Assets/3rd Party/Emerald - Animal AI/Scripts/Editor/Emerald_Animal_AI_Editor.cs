//Emerald - Animal AI by: Black Horizon Studios
//Version 1.1

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Emerald_Animal_AI))] 
[System.Serializable]
public class Emerald_Animal_AI_Editor : Editor 
{
	bool showHelpOptions = true;

	enum AgressionDropDown
	{
		Cowardly = 1,
		Farm = 2,
		Aggressive = 3
	}

	enum PreyOrPredator
	{
		Prey = 1,
		Predator = 2,
		None = 3
	}

	enum PreySize
	{
		Small = 1,
		Medium = 2,
		Large = 3
	}

	enum PredatorSize
	{
		Small = 1,
		Medium = 2,
		Large = 3
	}

	enum Alpha
	{
		Yes = 1,
		No = 2
	}

	AgressionDropDown editorAgression = AgressionDropDown.Cowardly;
	PreyOrPredator editorPreyOrPredator = PreyOrPredator.Prey;
	PreySize editorPreySize = PreySize.Medium;
	PredatorSize editorPredatorSize = PredatorSize.Medium;
	Alpha editorAlpha = Alpha.No;
	
	public override void OnInspectorGUI () 
	{
		Emerald_Animal_AI self = (Emerald_Animal_AI)target;

		float thirdOfScreen = Screen.width/3-10;
		int sizeOfHideButtons = 18;

		EditorGUILayout.LabelField("Emerald - Animal AI System Editor", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Editor Options", EditorStyles.boldLabel);
		string showOrHide = "Show";
		if(showHelpOptions)
			showOrHide = "Hide";
		if(GUILayout.Button(showOrHide+ " Help Boxes", GUILayout.Width(thirdOfScreen*2), GUILayout.Height(sizeOfHideButtons)) )
		{
			showHelpOptions = !showHelpOptions;
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Animal Options", EditorStyles.boldLabel);

		editorAgression = (AgressionDropDown)self.aggression;
		editorAgression = (AgressionDropDown)EditorGUILayout.EnumPopup("Behavior Type", editorAgression);
		self.aggression = (int)editorAgression;

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The Behavior Type determins whether your AI is a Wild or a Farm type animal. A Wild animal will flee when its flee tag is within its flee radius. A Farm animal will never flee and can have players near without being affected by them.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Animal Type Name", EditorStyles.miniLabel);
		self.animalNameType = EditorGUILayout.TextArea(self.animalNameType, GUILayout.MaxHeight(75));

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The Animal Type helps with Emerald dynamically forming herds and packs based on your Animal Name Type. So, only animals with the same Animal Name Type will create herds or packs.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		editorPreyOrPredator = (PreyOrPredator)self.preyOrPredator;
		editorPreyOrPredator = (PreyOrPredator)EditorGUILayout.EnumPopup("Prey or Predator?", editorPreyOrPredator);
		self.preyOrPredator = (int)editorPreyOrPredator;
		
		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("This allows you to choose if your animal is either Prey or Predator. If an animal is marked as Prey, a Predator will be able to chase and kill them, if the Predator size is less than or equal to their size.", MessageType.None, true);
		}
		
		EditorGUILayout.Space();

		if (self.preyOrPredator == 1)
		{
			editorPreySize = (PreySize)self.preySize;
			editorPreySize = (PreySize)EditorGUILayout.EnumPopup("Prey Size", editorPreySize);
			self.preySize = (int)editorPreySize;

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Prey Size determins how big your Prey animal is. The Prey Size must match a Predator's Size in order for that predator to be able to hunt the prey.", MessageType.None, true);
			}
		}

		if (self.preyOrPredator == 2)
		{
			editorPredatorSize = (PredatorSize)self.predatorSize;
			editorPredatorSize = (PredatorSize)EditorGUILayout.EnumPopup("Predator Size", editorPredatorSize);
			self.predatorSize = (int)editorPredatorSize;
			
			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Predator Size determins how big your Predator animal is. The Predator Size must be less than or equal to the Prey's Size in order for that predator to be able to hunt the prey.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			self.attackTime = EditorGUILayout.Slider ("Attack Speed", (float)self.attackTime, 0.5f, 6.0f);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Attack Speed controls how fast your animal can attack. Emerald calculates your animals animations to match your attack speed.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.LabelField("Herd Options", EditorStyles.boldLabel);

		EditorGUILayout.Space();
		
		self.autoGenerateAlpha = EditorGUILayout.Toggle ("Auto Generate Alpha?",self.autoGenerateAlpha);

		EditorGUILayout.Space();

		if (!self.autoGenerateAlpha)
		{
			editorAlpha = (Alpha)self.isAlphaOrNot;
			editorAlpha = (Alpha)EditorGUILayout.EnumPopup("Is this animal an Alpha?", editorAlpha);
			self.isAlphaOrNot = (int)editorAlpha;
		}

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("If Auto Generate Alpha is enabled, alphas will be generated automatically. There is a 1 in 5 chance of an animal being an alpha. If this option is disabled, you can customize which animals are alphas manually.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.maxPackSize = EditorGUILayout.IntSlider ("Max Pack Size", self.maxPackSize, 1, 10);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Controls the max pack size for this animal, if they're generated to become an alpha.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.offSetHerdXMin = EditorGUILayout.Slider ("offset Herd X Min", (float)self.offSetHerdXMin, -30, -5);
		self.offSetHerdXMax = EditorGUILayout.Slider ("offset Herd X Max", (float)self.offSetHerdXMax, 5, 30);

		EditorGUILayout.Space();

		self.offSetHerdZMin = EditorGUILayout.Slider ("offset Herd Z Min", (float)self.offSetHerdZMin, -15, -5);
		self.offSetHerdZMax = EditorGUILayout.Slider ("offset Herd Z Max", (float)self.offSetHerdZMax, 5, 15);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("These control the Offset of how the aniaml's herds form. The bigger the distance, both positive and negative, the more separation.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.alphaWaitForHerd = EditorGUILayout.Toggle ("Alpha Waits for Herd?",self.alphaWaitForHerd);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("This toggles whether or not you want the alpha to wait for its herd if a member's distance from the alpha becomes too far away.", MessageType.None, true);
		}

		if (self.alphaWaitForHerd)
		{
			EditorGUILayout.Space();

			self.maxDistanceFromHerd = EditorGUILayout.IntSlider ("Max Distance From Herd", (int)self.maxDistanceFromHerd, 1, 100);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("This controls when the alpha will wait for its herd. This happens when this distance is met.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.LabelField("Health Options", EditorStyles.boldLabel);

		self.startingHealth = EditorGUILayout.FloatField("Starting Health", self.startingHealth);

		EditorGUILayout.Space();

		self.currentHealth = EditorGUILayout.FloatField("Current Health", self.currentHealth);

		EditorGUILayout.Space();

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Here you can set your animal's health. When its health reaches 0, the animal will die and it will spawn a dead replacement.", MessageType.None, true);
		}

		if (self.preyOrPredator == 2)
		{
			self.attackDamage = EditorGUILayout.Slider ("Attack Damage", self.attackDamage, 0, 25.0f);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Attack Damage calls the Damage function within Emerald. The amount set above is the damage the animal will do.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();

		bool deadObject  = !EditorUtility.IsPersistent (self);
		self.deadObject = (GameObject)EditorGUILayout.ObjectField ("Dead Object", self.deadObject, typeof(GameObject), deadObject);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Here you can set your animal's dead GameObject replacement.", MessageType.None, true);
		}

		//Uncomment if you would like to add a hit effect (Ex: UFPS shake effect)
		/*
		EditorGUILayout.Space();

		bool hitEffect  = !EditorUtility.IsPersistent (self);
		self.hitEffect = (GameObject)EditorGUILayout.ObjectField ("Hit Effect", self.bloodEffect, typeof(GameObject), hitEffect);
		*/

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.LabelField("Sound Options Options", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		if (self.preyOrPredator == 2)
		{
			self.useAttackSound = EditorGUILayout.Toggle ("Use Attack Sound?",self.useAttackSound);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Attack Sound is the sound the animal will use when it attacks.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			if (self.useAttackSound)
			{
				bool attackSound  = !EditorUtility.IsPersistent (self);
				self.attackSound = (AudioClip)EditorGUILayout.ObjectField ("Attack Sound", self.attackSound, typeof(AudioClip), attackSound);
			}
		}

		EditorGUILayout.Space();

		self.useHitSound = EditorGUILayout.Toggle ("Use Get Hit Sound?",self.useHitSound);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The Hit Sound is the sound the animal will use when it receives damage.", MessageType.None, true);
		}

		if (self.useHitSound)
		{
			EditorGUILayout.Space();
			bool getHitSound  = !EditorUtility.IsPersistent (self);
			self.getHitSound = (AudioClip)EditorGUILayout.ObjectField ("Hit Sound", self.getHitSound, typeof(AudioClip), getHitSound);
		}

		EditorGUILayout.Space();

		if (self.aggression == 1)
		{
			self.playSoundOnFlee = EditorGUILayout.Toggle ("Play Flee Sound?",self.playSoundOnFlee);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Flee Sound is the sound the animal will use when it flees from a predator or player.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			if (self.playSoundOnFlee)
			{
				bool fleeSound  = !EditorUtility.IsPersistent (self);
				self.fleeSound = (AudioClip)EditorGUILayout.ObjectField ("Flee Sound", self.fleeSound, typeof(AudioClip), fleeSound);
			}
		}

		EditorGUILayout.Space();

		self.useRunSound = EditorGUILayout.Toggle ("Use Get Run Sound?",self.useRunSound);
		
		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The sound the animal uses when it runs.", MessageType.None, true);
		}


		EditorGUILayout.Space();

		if (self.useRunSound)
		{
			bool runSound  = !EditorUtility.IsPersistent (self);
			self.runSound = (AudioClip)EditorGUILayout.ObjectField ("Run Sound", self.runSound, typeof(AudioClip), runSound);

			EditorGUILayout.Space();

			self.footStepSeconds = EditorGUILayout.Slider ("Footstep Seconds", (float)self.footStepSeconds, 0.1f, 1.0f);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Footstep Seconds controls the seconds in between each time the sound is playing.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();
		
		self.minSoundPitch = EditorGUILayout.Slider ("Min Sound Pitch", (float)self.minSoundPitch, 0.5f, 1.5f);
		self.maxSoundPitch = EditorGUILayout.Slider ("Max Sound Pitch", (float)self.maxSoundPitch, 0.5f, 1.5f);
		
		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("This controls the min and max sound pitch for the animal's AudioSource. This affects all sounds adding various pitches to each animal keeping them unique.", MessageType.None, true);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.LabelField("Pathfinding Options", EditorStyles.boldLabel);

		//Hold
		//self.maxNumberOfActiveAnimals = EditorGUILayout.IntSlider ("Max Active Aniamls", self.maxNumberOfActiveAnimals, 1, 100);
		
		self.drawWaypoints = EditorGUILayout.Toggle ("Draw Waypoints?",self.drawWaypoints);
		
		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Draw Waypoints determins if the AI will draw its current waypoint/destination. This can make it helpful for development/testing.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.drawPaths = EditorGUILayout.Toggle ("Draw Paths?",self.drawPaths);
		
		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Draw Paths determins if the AI will draw its current path to its current destination. This can make it helpful for development/testing.", MessageType.None, true);
		}

		if (self.drawPaths)
		{
			EditorGUILayout.Space();
			
			self.pathWidth = EditorGUILayout.Slider ("Path Line Width", (float)self.pathWidth, 1.0f, 100.0f);
			
			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Choose how wide you would like your Path Lines drawn.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			self.lineYOffSet = EditorGUILayout.Slider ("Path Line Y Offset", (float)self.lineYOffSet, 0.0f, 5.0f);
			
			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Path Line Y Offset will offset your path line on the Y axis based on the amount used on the slider above. This is useful if the Path Line is too high or too low.", MessageType.None, true);
			}
		}

		if (self.drawPaths)
		{
			EditorGUILayout.Space();

			self.pathColor = EditorGUILayout.ColorField("Path Line Color", self.pathColor);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Path Line Color allows you to customize what color you want the path lines to be.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			bool pathMaterial  = !EditorUtility.IsPersistent (self);
			self.pathMaterial = (Material)EditorGUILayout.ObjectField ("Path Line Material", self.pathMaterial, typeof(Material), pathMaterial);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Choose the material you want to be used for your Path Line. If no material is applied, a default one will be used. Note: The color of the default material is purple and can't be adjusted.", MessageType.None, true);
			}

		}

		EditorGUILayout.Space();

		/*
		self.enableDebugLogs = EditorGUILayout.Toggle ("Enable Debug Logs?",self.enableDebugLogs);
		
		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Enable Debug Logs can be useful to help balance your ecosystem. When an initial hunt or flee is triggered, it tells what's happening by the predator or prey's name.", MessageType.None, true);
		}
		*/

		EditorGUILayout.Space();

		self.alignAI = EditorGUILayout.Toggle ("Align AI?",self.alignAI);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("If Align AI is enabled, it will automatically, and smoothly, align AI to the slope of the terrain. This allows much more realistic results.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.updateSpeed = EditorGUILayout.Slider ("Update Speed", self.updateSpeed, 0.01f, 2.0f);
		
		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Update Speed determines how often culling is checked. If an animal is culled, it will be disabled to increase performance. If an animal is visible, it will enable all components on that animal. The less often this option is updated, the more it increases performance, but animals may not react the second a player looks at them. So, it's best to balance this option with performance and playing quality.", MessageType.None, true);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Range Options", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		self.useVisualRadius = EditorGUILayout.Toggle ("Use Visual Radiuses?",self.useVisualRadius);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Using Visual Radiuses will visually render the radiuses in the scene view. This makes it easy to see where your AI's Ranges are.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.stoppingDistance = EditorGUILayout.Slider ("Stopping Distance", self.stoppingDistance, 0.1f, 10.0f);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The Stopping Distance determins the distance in which your AI will stop for its waypoint/destination.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		if (self.aggression == 1)
		{
			self.fleeRadius = EditorGUILayout.IntSlider ("Flee Trigger Radius", self.fleeRadius, 1, 100);

			EditorGUILayout.Space();

			self.fleeRadiusColor = EditorGUILayout.ColorField("Flee Radius Color", self.fleeRadiusColor);
		
			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Flee Trigger Radius is the radius in which the AI will be triggered to flee.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			self.extraFleeSeconds = EditorGUILayout.Slider ("Extra Flee Seconds", (float)self.extraFleeSeconds, 0f, 10.0f);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Extra Flee Seconds controls how many extra seconds the Animal AI will be in Flee Mode before it stops.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			self.chaseSeconds = EditorGUILayout.IntSlider ("Chase Seconds", (int)self.chaseSeconds, 1, 60);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Chase Seconds determines how long an animal can flee before being exhausted and switching to cooldown mode.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			self.coolDownSeconds = EditorGUILayout.Slider ("Cool Down Seconds", self.coolDownSeconds, 0, 25);
			
			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Cool Down Seconds controls how many seconds your animal will wait after they have reached their chase seconds. The animal will then return back to its statring position while in this mode.", MessageType.None, true);
			}
		}

		if (self.aggression == 3)
		{
			self.huntRadius = EditorGUILayout.IntSlider ("Hunt Trigger Radius", self.huntRadius, 1, 200);
			
			EditorGUILayout.Space();
			
			self.huntRadiusColor = EditorGUILayout.ColorField("Hunt Radius Color", self.huntRadiusColor);
			
			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Hunt Trigger Radius is the radius in which the AI will be triggered to hunt. This process is pause if the animal is within attacking distance.", MessageType.None, true);
			}

			self.huntSeconds = EditorGUILayout.Slider ("Hunt Seconds", self.huntSeconds, 1, 50);

			EditorGUILayout.Space();

			self.coolDownSeconds = EditorGUILayout.Slider ("Cool Down Seconds", self.coolDownSeconds, 0, 25);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Cool Down Seconds controls how many seconds your animal will wait after they have reached their max Hunt Seconds. During the Cool Down phase, the animal will return back to its statring position. The seconds for this are rest if an animal receives damage.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();

		self.wanderRange = EditorGUILayout.IntSlider ("Wander Range", self.wanderRange, 1, 500);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Wander Range controls the radius in which the animal will wander. It will not wander out of its Wander Range, unless it's fleeing.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.wanderRangeColor = EditorGUILayout.ColorField("Wander Range Color", self.wanderRangeColor);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The Wander Range is a radius that the AI will wander in from its originally placed spot. It will not wander further than this radius.", MessageType.None, true);
		}

		if (self.aggression == 1)
		{
			EditorGUILayout.Space();
			
			self.freezeSecondsMin = EditorGUILayout.Slider ("Min Freeze Seconds", (float)self.freezeSecondsMin, 0.25f, 3.0f);
			self.freezeSecondsMax = EditorGUILayout.Slider ("Max Freeze Seconds", (float)self.freezeSecondsMax, 0.5f, 8.0f);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("This controls the min and max seconds it takes for the animals to react to a predator or player triggering the animal to flee.", MessageType.None, true);
			}

			/*
			EditorGUILayout.Space();
			self.fleeRange = EditorGUILayout.IntSlider ("Flee Range", self.fleeRange, 1, 100);
			self.fleeRangeColor = EditorGUILayout.ColorField("Flee Range Color", self.fleeRangeColor);
	
			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Flee Range is a radius that the AI will flee in from its current placed spot. It may wander further than this radius, if being chased.", MessageType.None, true);
			}
			*/
		}

		EditorGUILayout.Space();

		self.grazeLengthMin = EditorGUILayout.IntSlider ("Graze Length Min", self.grazeLengthMin, 1, 100);
		self.grazeLengthMax = EditorGUILayout.IntSlider ("Graze Length Max", self.grazeLengthMax, 1, 100);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The graze lengths are generated with the min and max values entered above. This also plays a role in how often the waypoints are changed. If the AI is unable to reach its waypoint within its generated graze length time, a new waypoint will be generated.", MessageType.None, true);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Movement Options", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Here you can adjust the speeds that your AI will use. This AI system uses a NavMeshAgent, which is applied automatically. These speeds will change the NavMeshAgent's speed when a AI goes into flee mode.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.walkSpeed = EditorGUILayout.Slider ("Walk Speed", (float)self.walkSpeed, 0.5f, 10.0f);
		self.runSpeed = EditorGUILayout.Slider ("Run Speed", (float)self.runSpeed, 3.0f, 15.0f);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("These control how fast the animals will walk and run.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.turnSpeed = EditorGUILayout.IntSlider ("Turn Speed", self.turnSpeed, 1, 2000);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("The Turn Speed determins how fast your AI will rotate to its waypoint/destination when a waypoint is generated.", MessageType.None, true);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Animation Options", EditorStyles.boldLabel);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Here you can setup your AI's animations. You simple type the name of the animation you'd like to use below and the system will use them for the selected animations.", MessageType.None, true);
		}

		EditorGUILayout.Space();

		self.useAnimations = EditorGUILayout.Toggle ("Use Animations?",self.useAnimations);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("If the animal does not use animations, animations can be disabled here. However, they will be disabled automatically if no animation component is found on the current model.", MessageType.None, true);
		}

		if (self.useAnimations)
		{

			EditorGUILayout.Space();

			bool idleAnimation  = !EditorUtility.IsPersistent (self);
			self.idleAnimation = (AnimationClip)EditorGUILayout.ObjectField ("Idle Animation", self.idleAnimation, typeof(AnimationClip), idleAnimation);

			if (self.preyOrPredator == 2)
			{
				EditorGUILayout.Space();

				bool idleBattleAnimation  = !EditorUtility.IsPersistent (self);
				self.idleBattleAnimation = (AnimationClip)EditorGUILayout.ObjectField ("Idle (Aggressive) Animation", self.idleBattleAnimation, typeof(AnimationClip), idleBattleAnimation);

				if (showHelpOptions == true)
				{
					EditorGUILayout.HelpBox("If an animal doesn't have an aggressive idle animation, you can just use your basic idle animation.", MessageType.None, true);
				}
			}

			EditorGUILayout.Space();

			self.totalGrazeAnimations = EditorGUILayout.IntSlider ("Total Graze Animations", self.totalGrazeAnimations, 1, 3);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("Total Graze Animations determins how many graze animations your AI will use when wandering. These animations will be picked at random. These can also be Idle animations, if desired. There is a max of 3.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			bool graze1Animation  = !EditorUtility.IsPersistent (self);
			self.graze1Animation = (AnimationClip)EditorGUILayout.ObjectField ("Graze 1 Animation", self.graze1Animation, typeof(AnimationClip), graze1Animation);

			if (self.totalGrazeAnimations == 2 || self.totalGrazeAnimations == 3)
			{
				bool graze2Animation  = !EditorUtility.IsPersistent (self);
				self.graze2Animation = (AnimationClip)EditorGUILayout.ObjectField ("Graze 2 Animation", self.graze2Animation, typeof(AnimationClip), graze2Animation);
			}
			
			if (self.totalGrazeAnimations == 3)
			{
				bool graze3Animation  = !EditorUtility.IsPersistent (self);
				self.graze3Animation = (AnimationClip)EditorGUILayout.ObjectField ("Graze 3 Animation", self.graze3Animation, typeof(AnimationClip), graze3Animation);
			}

			EditorGUILayout.Space();

			bool walkAnimation  = !EditorUtility.IsPersistent (self);
			self.walkAnimation = (AnimationClip)EditorGUILayout.ObjectField ("Walk Animation", self.walkAnimation, typeof(AnimationClip), walkAnimation);

			bool runAnimation  = !EditorUtility.IsPersistent (self);
			self.runAnimation = (AnimationClip)EditorGUILayout.ObjectField ("Run Animation", self.runAnimation, typeof(AnimationClip), runAnimation);

			//EditorGUILayout.LabelField("Death Animation Name", EditorStyles.miniLabel);
			//self.deathAnimationName = EditorGUILayout.TextArea(self.deathAnimationName, GUILayout.MaxHeight(75));

			EditorGUILayout.Space();

			bool hitAnimation  = !EditorUtility.IsPersistent (self);
			self.hitAnimation = (AnimationClip)EditorGUILayout.ObjectField ("Hit Animation", self.hitAnimation, typeof(AnimationClip), hitAnimation);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The hit animation that is played when the animal receives damage.", MessageType.None, true);
			}

			EditorGUILayout.Space();

			if (self.preyOrPredator == 2)
			{
				self.useRunAttackAnimations = EditorGUILayout.Toggle ("Use Run Attack Animations?",self.useRunAttackAnimations);

				if (showHelpOptions == true)
				{
					EditorGUILayout.HelpBox("If the animal has run attack animation, it can be set here. It will then be played when a predator is attacking a target while running.", MessageType.None, true);
				}
			}
			
			EditorGUILayout.Space();

			if (self.useRunAttackAnimations)
			{
				bool runAttackAnimation  = !EditorUtility.IsPersistent (self);
				self.runAttackAnimation = (AnimationClip)EditorGUILayout.ObjectField ("Run Attack Animation", self.runAttackAnimation, typeof(AnimationClip), runAttackAnimation);
			}

			EditorGUILayout.Space();

			if (self.preyOrPredator == 2)
			{
				self.totalAttackAnimations = EditorGUILayout.IntSlider ("Total Attack Animations", self.totalAttackAnimations, 1, 6);

				if (showHelpOptions == true)
				{
					EditorGUILayout.HelpBox("Here you control how many attack animations your predator uses. There are a max of 6. Each will be used randomly when the animal is attacking.", MessageType.None, true);
				}

				EditorGUILayout.Space();

				bool attackAnimation1  = !EditorUtility.IsPersistent (self);
				self.attackAnimation1 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 1", self.attackAnimation1, typeof(AnimationClip), attackAnimation1);

				if (self.totalAttackAnimations == 2)
				{
					bool attackAnimation2  = !EditorUtility.IsPersistent (self);
					self.attackAnimation2 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 2", self.attackAnimation2, typeof(AnimationClip), attackAnimation2);
				}

				if (self.totalAttackAnimations == 3)
				{
					bool attackAnimation2  = !EditorUtility.IsPersistent (self);
					self.attackAnimation2 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 2", self.attackAnimation2, typeof(AnimationClip), attackAnimation2);

					bool attackAnimation3  = !EditorUtility.IsPersistent (self);
					self.attackAnimation3 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 3", self.attackAnimation3, typeof(AnimationClip), attackAnimation3);
				}

				if (self.totalAttackAnimations == 4)
				{
					bool attackAnimation2  = !EditorUtility.IsPersistent (self);
					self.attackAnimation2 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 2", self.attackAnimation2, typeof(AnimationClip), attackAnimation2);

					bool attackAnimation3  = !EditorUtility.IsPersistent (self);
					self.attackAnimation3 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 3", self.attackAnimation3, typeof(AnimationClip), attackAnimation3);

					bool attackAnimation4  = !EditorUtility.IsPersistent (self);
					self.attackAnimation4 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 4", self.attackAnimation4, typeof(AnimationClip), attackAnimation4);
				}

				if (self.totalAttackAnimations == 5)
				{
					bool attackAnimation2  = !EditorUtility.IsPersistent (self);
					self.attackAnimation2 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 2", self.attackAnimation2, typeof(AnimationClip), attackAnimation2);
					
					bool attackAnimation3  = !EditorUtility.IsPersistent (self);
					self.attackAnimation3 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 3", self.attackAnimation3, typeof(AnimationClip), attackAnimation3);

					bool attackAnimation4  = !EditorUtility.IsPersistent (self);
					self.attackAnimation4 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 4", self.attackAnimation4, typeof(AnimationClip), attackAnimation4);

					bool attackAnimation5  = !EditorUtility.IsPersistent (self);
					self.attackAnimation5 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 5", self.attackAnimation5, typeof(AnimationClip), attackAnimation5);
				}

				if (self.totalAttackAnimations == 6)
				{
					bool attackAnimation2  = !EditorUtility.IsPersistent (self);
					self.attackAnimation2 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 2", self.attackAnimation2, typeof(AnimationClip), attackAnimation2);
					
					bool attackAnimation3  = !EditorUtility.IsPersistent (self);
					self.attackAnimation3 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 3", self.attackAnimation3, typeof(AnimationClip), attackAnimation3);
					
					bool attackAnimation4  = !EditorUtility.IsPersistent (self);
					self.attackAnimation4 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 4", self.attackAnimation4, typeof(AnimationClip), attackAnimation4);
					
					bool attackAnimation5  = !EditorUtility.IsPersistent (self);
					self.attackAnimation5 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 5", self.attackAnimation5, typeof(AnimationClip), attackAnimation5);

					bool attackAnimation6  = !EditorUtility.IsPersistent (self);
					self.attackAnimation6 = (AnimationClip)EditorGUILayout.ObjectField ("Attack Animation 6", self.attackAnimation6, typeof(AnimationClip), attackAnimation6);
				}
			}
			EditorGUILayout.Space();

			self.walkAnimationSpeed = EditorGUILayout.Slider ("Walk Animation Speed", (float)self.walkAnimationSpeed, 0.5f, 2.0f);
			self.runAnimationSpeed = EditorGUILayout.Slider ("Run Animation Speed", (float)self.runAnimationSpeed, 0.5f, 2.0f);

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("These control the actual speed that the animations play.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.LabelField("Tag Options", EditorStyles.boldLabel);

		if (self.aggression == 1)
		{
			EditorGUILayout.LabelField("Flee Tag Name", EditorStyles.miniLabel);
			self.playerTagName = EditorGUILayout.TextArea(self.playerTagName, GUILayout.MaxHeight(75));

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The Flee Tag is the tag that triggers the AI's to flee. This is usually your player's tag.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();

		if (self.preyOrPredator == 2)
		{
			EditorGUILayout.LabelField("Prey Tag Name", EditorStyles.miniLabel);
			self.preyTagName = EditorGUILayout.TextArea(self.preyTagName, GUILayout.MaxHeight(75));

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The tag name of animals that are Prey.", MessageType.None, true);
			}
		}

		EditorGUILayout.Space();

		if (self.preyOrPredator == 1)
		{
			EditorGUILayout.LabelField("Predator Tag Name", EditorStyles.miniLabel);
			self.predatorTagName = EditorGUILayout.TextArea(self.predatorTagName, GUILayout.MaxHeight(75));

			if (showHelpOptions == true)
			{
				EditorGUILayout.HelpBox("The tag name of animals that are Predators. This will also cause prey animals to flee.", MessageType.None, true);
			}
		}



		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Effect Options", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		self.useDustEffect = EditorGUILayout.Toggle ("Use Dust Effect?",self.useDustEffect);

		if (showHelpOptions == true)
		{
			EditorGUILayout.HelpBox("Use Dust Effect determins if your player uses a dust effect when running.", MessageType.None, true);
		}

		if (self.useDustEffect)
		{
			EditorGUILayout.Space();

			bool dustEffect = !EditorUtility.IsPersistent (self);
			self.dustEffect = (ParticleSystem)EditorGUILayout.ObjectField ("Dust Effect", self.dustEffect, typeof(ParticleSystem), dustEffect);
		}

		if (GUI.changed) 
		{ 
			EditorUtility.SetDirty(self); 
		}
		
		serializedObject.ApplyModifiedProperties ();
	}

	void OnSceneGUI () 
	{
		Emerald_Animal_AI self = (Emerald_Animal_AI)target;

		if (self.aggression == 1 && self.useVisualRadius)
		{
			Handles.color = self.fleeRadiusColor;
			Handles.DrawSolidDisc(self.transform.position, Vector3.up, self.fleeRadius);

			Handles.color = self.wanderRangeColor;
			Handles.DrawSolidDisc(self.transform.position, Vector3.up, self.wanderRange);

			Handles.color = self.fleeRangeColor;
			Handles.DrawSolidDisc(self.transform.position, Vector3.up, self.fleeRange);
		}

		if (self.aggression == 3 && self.useVisualRadius)
		{
			Handles.color = self.huntRadiusColor;
			Handles.DrawSolidDisc(self.transform.position, Vector3.up, self.huntRadius);
			
			Handles.color = self.wanderRangeColor;
			Handles.DrawSolidDisc(self.transform.position, Vector3.up, self.wanderRange);
		}

		if (self.aggression == 2 && self.useVisualRadius)
		{
			Handles.color = self.wanderRangeColor;
			Handles.DrawSolidDisc(self.transform.position, Vector3.up, self.wanderRange);
		}

		SceneView.RepaintAll();
	}
	
}



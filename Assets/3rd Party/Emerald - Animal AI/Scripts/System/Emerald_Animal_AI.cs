//Emerald - Animal AI by: Black Horizon Studios
//Version 1.1

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (SphereCollider))]
[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (AudioSource))]

public class Emerald_Animal_AI : MonoBehaviour 
{
	public int fleeRadius = 15;
	public int huntRadius = 15;
	public int wanderRange = 20;
	public int fleeRange = 25;
	public int turnSpeed = 800;
	public float stoppingDistance = 2;
	public float extraFleeSeconds = 5;

	public int grazeLengthMin = 2;
	public int grazeLengthMax = 6;

	public float walkSpeed = 4.0f;
	public float runSpeed = 8.0f;

	public float walkAnimationSpeed = 1.0f;
	public float runAnimationSpeed = 1.0f;

	public AnimationClip idleAnimation;
	public AnimationClip idleBattleAnimation;
	public AnimationClip graze1Animation;
	public AnimationClip graze2Animation;
	public AnimationClip graze3Animation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public AnimationClip deathAnimation;

	public AnimationClip[] grazeAnimations;

	public string playerTagName = "Player";
	public string preyTagName = "Huntable";
	public string predatorTagName = "Predator";

	public float pathWidth = 0.25f;
	public Color pathColor = Color.green;
	
	public bool drawWaypoints = false;
	public bool drawPaths = false;
	public bool useDustEffect = false;

	public ParticleSystem dustEffect;

	public Color wanderRangeColor = new Color(0f, 0.8f, 0, 0.1f);
	public Color fleeRadiusColor = new Color(1.0f, 1.0f, 0, 0.1f);
	public Color huntRadiusColor = new Color(1.0f, 1.0f, 0, 0.1f);
	public Color fleeRangeColor = new Color(1.0f, 0, 0, 0.1f);

	public bool isFleeing = false;
	public bool isGrazing = false;
	private NavMeshAgent navMeshAgent;
	private float timer = 0;
	private int grazeLength = 10;
	private Vector3 startPosition;
	private Vector3 currentPlayerTransform;
	private float playerZPos;
	private Vector3 destination;
	private float pathWidthAdjusted;

	private GameObject currentWaypoint;
	private GameObject fleePoint;

	private LineRenderer line;
	private Transform target; 
	private ParticleSystem clone;
	private Animation anim;
	private SphereCollider triggerCollider;
	private BoxCollider boxCollider;

	public int aggression = 1;
	public int grazeAnimationNumber;
	public int totalGrazeAnimations;
	public Material pathMaterial;
	public float lineYOffSet;
	public bool useVisualRadius = true;
	public bool useAnimations = true;
	public bool rotateFlee = false;
	public Vector3 direction;
	public Quaternion playerRotation;
	public Quaternion predatorRotation;
	public float rotateTimer;
	public float fleeTimer;
	public bool startFleeTimer = false;

	public Terrain terrain;
	public GameObject terrainGameObeject;
	public float steepness;
	public float dest;
	public GameObject thing;

	public bool huntMode = false;
	public GameObject currentAnimal;
	public int preySize = 2;
	public int predatorSize = 2;
	public bool startHuntTimer = false;
	public float huntTimer = 30;
	public float huntSeconds = 5;
	public int preyOrPredator = 1;
	public bool preySizeMatched = false;
	public bool enableDebugLogs = false;
	public float attackTimer = 0;
	public float attackTime = 1;
	public bool withinAttackDistance = false;

	public int totalAttackAnimations;
	public int currentAttackAnimation;
	public AnimationClip currentAttackAnimationClip;
	public AnimationClip attackAnimation1;
	public AnimationClip attackAnimation2;
	public AnimationClip attackAnimation3;
	public AnimationClip attackAnimation4;
	public AnimationClip attackAnimation5;
	public AnimationClip attackAnimation6;
	public AnimationClip hitAnimation;
	public AnimationClip runAttackAnimation;

	public GameObject hitEffect;
	public bool damageDealt = false;
	public bool damageTaken = false;
	public bool useRunAttackAnimations = false;
	public float startingHealth = 15;
	public float currentHealth = 15;
	public GameObject deadObject;

	public int offSetPosition;
	public int offSetDistance;
	public bool currentlyBeingPursued = false;
	public float attackDamage = 5;

	public AudioClip attackSound;
	public AudioSource audioSource;
	public AudioClip getHitSound;
	public float minSoundPitch = 0.8f;
	public float maxSoundPitch = 1.2f;
	public float updateSpeedTimer = 0;
	public float updateSpeed = 0.1f;

	public float velocity;
	public bool attackWhileRunning = false;
	public bool isCoolingDown = false;
	public float coolDownTimer;
	public float coolDownSeconds = 25;
	public Quaternion lookRotation;
	public Quaternion originalLookRotation;

	public float freezeSecondsMin = 0.25f;
	public float freezeSecondsMax = 1;
	public float freezeSecondsTotal;
	public float freezeSecondsTimer = 0;
	public bool isFrozen = false;

	//Global Stats
	public int maxNumberOfActiveAnimals = 10;
	public static int currentNumberOfActiveAnimals;
	public bool systemOn = false;
	public Renderer objectsRender;

	public float updateSystemSpeed = 1;
	public float updateSystemTimer = 1;
	public float navMeshCountDownTimer = 0;
	public bool navMeshCountDown = false;
	public bool navMeshDisabled = true;

	public bool inHerd = false;
	public Transform animalToFollow;
	public int isAlpha;
	public int isAlphaOrNot = 2;
	public float offSetHerdX;
	public float offSetHerdXMin = -10;
	public float offSetHerdXMax = 10;
	public float offSetHerdZ;
	public float offSetHerdZMin = -10;
	public float offSetHerdZMax = 10;
	public string animalNameType = "";
	public bool threatIsOutOfTigger = false;
	public List <GameObject> herdList = new List<GameObject>();
	public int herdNumber;
	public bool markInPack = false;
	public GameObject temp;
	public bool isDead = false;

	public GameObject fleeTarget;
	public bool calculateFlee = false; 
	public bool playSoundOnFlee = false;
	public AudioClip fleeSound;
	public Vector3 Direction;
	public bool distantFlee;
	public bool calculateWander = false;
	public float footStepSeconds = 0.15f;
	public float runTimer = 0;
	public AudioClip runSound;
	public int maxPackSize = 5;
	public bool packCreated = false;
	public int maxDistanceFromHerd = 100;
	public bool hasPack = false;
	public bool isExhausted = false;
	public float chaseTimer;
	public int chaseSeconds = 60;
	public bool herdFull = false;
	public bool waitingForHerd = false;
	public bool alignAI = true;
	public bool terrainFound = false;
	public Transform alignTarget;
	public bool alphaWaitForHerd = false;
	public bool attackingEnabled = false;

	public bool useHitSound = false;
	public bool useAttackSound = false;
	public bool useRunSound = false;
	public bool autoGenerateAlpha = true;

	void Awake()
	{
		startPosition = this.transform.position;

		if (!isFleeing)
		{
			grazeLength = Random.Range(grazeLengthMin, grazeLengthMax);
		}

		if (isFleeing)
		{
			grazeLength = Random.Range(grazeLengthMin, grazeLengthMax);
		}
	}


	
	void Start () 
	{
		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		triggerCollider = GetComponent<SphereCollider>();
		boxCollider = GetComponent<BoxCollider>();
		gameObject.GetComponent<Rigidbody>().isKinematic = true;
		gameObject.GetComponent<Rigidbody>().useGravity = false;
		audioSource = GetComponent<AudioSource>();
		terrainGameObeject = GameObject.Find("Terrain");

		if (terrainGameObeject == null)
		{
			terrainFound = false;
			alignAI = false;
		}

		if (terrainGameObeject != null)
		{
			terrainFound = true;
		}

		if (terrainFound && alignAI)
		{
			terrain  = terrainGameObeject.GetComponent<Terrain>();
		}

		objectsRender = GetComponentInChildren<Renderer>();

		huntTimer = huntSeconds;

		if (GetComponent<LineRenderer>() == null && drawPaths)
		{
			this.gameObject.AddComponent<LineRenderer>();
		}

		triggerCollider.isTrigger = true;

		if (aggression == 1 || aggression == 2)
		{
			triggerCollider.radius = fleeRadius * 0.7f;
		}
		
		if (aggression == 3)
		{
			triggerCollider.radius = huntRadius * 0.7f;
		}

		if (drawWaypoints)
		{
			currentWaypoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
			currentWaypoint.gameObject.transform.localScale = new Vector3 (0.15f, 2.5f, 0.15f);
			currentWaypoint.name = this.gameObject.name + " Waypoint";
			currentWaypoint.GetComponent<BoxCollider>().isTrigger = true;
			currentWaypoint.AddComponent<AlignWaypoint>();
		}

		if (drawPaths)
		{
			line = GetComponent<LineRenderer>();
			line.material = pathMaterial;
		}

		navMeshAgent.angularSpeed = turnSpeed;
		navMeshAgent.stoppingDistance = stoppingDistance;

		currentHealth = startingHealth;
		currentAttackAnimation = Random.Range(1, totalAttackAnimations+1);

		if (useAnimations)
		{
			anim = GetComponent<Animation>();

			if (anim != null)
			{
				anim[walkAnimation.name].speed = walkAnimationSpeed;
				anim[runAnimation.name].speed = runAnimationSpeed;
			}
		}

		if (anim == null)
		{
			useAnimations = false;
		}

		if(useDustEffect)
		{
			clone = Instantiate(dustEffect, new Vector3 (transform.position.x, transform.position.y + 0.35f, transform.position.z + 1.0f), Quaternion.identity) as ParticleSystem;
			clone.transform.parent = transform;
			clone.emissionRate = 0;
		}

		if (drawPaths)
		{
			pathWidthAdjusted = pathWidth * 0.01f;

			line.SetWidth(pathWidthAdjusted, pathWidthAdjusted);
			line.SetColors(pathColor, pathColor);
		}

		if (drawPaths)
		{
			GetPath();
		}

		if (aggression == 2)
		{
			Wander();
			//isGrazingisGrazing = true;
		}
		//isCoolingDown = true;
		ApplyAttackAnimations();
		grazeAnimationNumber = Random.Range(1,totalGrazeAnimations+1);
		audioSource.pitch = Random.Range(minSoundPitch, maxSoundPitch);

		triggerCollider.enabled = false;
		navMeshAgent.enabled = false;
		boxCollider.isTrigger = true;
		boxCollider.enabled = false;

		if (autoGenerateAlpha)
		{
			isAlpha = Random.Range(0,5);

			//Not Alpha
			if (isAlpha <= 3)
			{
				isAlpha = 0;
			}
			
			//Is Alpha
			if (isAlpha > 3)
			{
				isAlpha = 1;
			}
		}

		if (!autoGenerateAlpha)
		{
			if (isAlphaOrNot == 1)
			{
				isAlpha = 1;
			}

			if (isAlphaOrNot == 2)
			{
				isAlpha = 0;
			}
		}



		offSetHerdX = Random.Range(offSetHerdXMin,offSetHerdXMax);
		offSetHerdZ = Random.Range(offSetHerdZMin,offSetHerdZMax);

		fleeTimer = extraFleeSeconds;
		maxPackSize = maxPackSize - 1;
	
		audioSource.enabled = false;

		//Waypoints need to be enabled in order for paths to be drawn
		if (drawPaths)
		{
			drawWaypoints = true;
		}

		if (useAnimations)
		{
			anim.enabled = false;
		}

		if (alignAI)
		{
			originalLookRotation = transform.rotation; 
			alignTarget = this.transform;
			navMeshAgent.updateRotation = false;
		}
	}



	void Wander()
	{
		if (systemOn && !isFleeing)
		{
			destination = startPosition + new Vector3(Random.Range (-wanderRange * 0.5f, wanderRange * 0.5f), 0, Random.Range (-wanderRange * 0.5f, wanderRange * 0.5f));
			NewDestination(destination);

			if (useAnimations && !isCoolingDown)
			{
				anim.CrossFade(walkAnimation.name);
			}

			isGrazing = false;

			navMeshAgent.speed = walkSpeed;

			if (drawWaypoints)
			{
				currentWaypoint.transform.position = destination;
			}
		}
	}
	
	void Flee()
	{
		isGrazing = false;

		//If our velocity drops near 0, play our idle animation
		if (useAnimations && !withinAttackDistance)
		{
			velocity = navMeshAgent.velocity.sqrMagnitude;
			
			if (velocity <= 0.05f && anim.IsPlaying(runAnimation.name))
			{
				anim.CrossFade(idleAnimation.name);
			}
		}

		if (isAlpha == 0 && inHerd && fleeTarget == null && preyOrPredator == 1)
		{
			fleeTarget = animalToFollow.GetComponent<Emerald_Animal_AI>().fleeTarget;
		}

		if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && !distantFlee)
		{
			if (useAnimations)
			{
				anim.CrossFade(runAnimation.name);
				navMeshAgent.speed = runSpeed;
			}

			if (!useAnimations)
			{
				navMeshAgent.speed = runSpeed;
			}

			Direction = transform.position - fleeTarget.transform.position;
			navMeshAgent.SetDestination(fleeTarget.transform.position + (Direction.normalized * fleeRadius));
			
			if (drawWaypoints)
			{
				currentWaypoint.transform.position = fleeTarget.transform.position + (Direction.normalized * fleeRadius);
			}
		}
		
		if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && !distantFlee)
		{
			distantFlee = true;
		}

		//Flee away from our target after our rotations have been calculated
		if (distantFlee && fleeTarget != null)
		{
			if (useAnimations)
			{
				anim.CrossFade(runAnimation.name);
				navMeshAgent.speed = runSpeed;
			}

			if (!useAnimations)
			{
				navMeshAgent.speed = runSpeed;
			}

			if (!navMeshAgent.hasPath)
			{
				destination = transform.position + transform.forward * 50;
				NewDestination(destination);
			}
			
			if (!navMeshAgent.isOnNavMesh)
			{
				destination = transform.position + transform.forward * 50;
				NewDestination(destination);
			}
			
			if (navMeshAgent.isPathStale)
			{
				destination = transform.position + transform.forward * 50;
				NewDestination(destination);
			}
			
			if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && !isDead)
			{
				destination = transform.position + transform.forward * 50;
				NewDestination(destination);
				
				if (drawWaypoints)
				{
					currentWaypoint.transform.position = destination;
				}
			}
		}
	}

	public void NewDestination(Vector3 targetPoint)
	{
		if (navMeshAgent.enabled)
		{
			navMeshAgent.SetDestination (targetPoint);
		}
	}

	//Calculates our path lines, if they are enabled
	void GetPath()
	{
		if (drawPaths)
		{
			line.SetPosition(0, new Vector3(transform.localPosition.x, transform.position.y + lineYOffSet, transform.position.z)); //set the line's origin
			DrawPath(navMeshAgent.path);
		}
	}

	//Draws our path lines
	public void DrawPath(NavMeshPath path)
	{
		if (drawPaths)
		{
			if(path.corners.Length < 1) 
				return;
			
			line.SetVertexCount(path.corners.Length); 
			
			for(int i = 1; i < path.corners.Length; i++)
			{
				line.SetPosition(i, path.corners[i]);
			}
		}
	}

	//Checks to see if our AI are still in view every few seconds.
	//If they are no longer in view, according to Unity's LOD System, disable all components until back in view.
	//Using the NavMesh timer, check to see that our AI are off screen for at least 10 seconds before deactivating.
	//If the AI are out of view, set systemOn to false, which stops Emerald from calculating
	void SystemOptimizerUpdater ()
	{
		updateSystemTimer += Time.deltaTime;			//Use this to keep track of the amount of seconds until we can update
		
		if (updateSystemTimer >= updateSystemSpeed)			//If the update seconds have reached the amount of desired update seconds, update our system optimizer						
		{
			if (objectsRender.isVisible)					//If the AI's renderer is visible, enabled our components
			{
				systemOn = true;
				navMeshAgent.enabled = true;
				navMeshDisabled = false;
				triggerCollider.enabled = true;
				boxCollider.enabled = true;
				audioSource.enabled = true;

				if (useAnimations)
				{
					anim.enabled = true;
				}

				navMeshCountDownTimer = 0;
				updateSystemTimer = 0;						//The updateSystemTimer has be ticked, restart.
			}
			
			if (!objectsRender.isVisible)				//If the AI's renderer is not visible, disable our components so that they aren't waisting performance when they aren't visible		
			{
				triggerCollider.enabled = false;
				boxCollider.enabled = false;
				navMeshCountDown = true;				
				updateSystemTimer = 0;						//The updateSystemTimer has be ticked, restart.
			}
		}
		
		if (navMeshCountDown && !navMeshDisabled)	
		{													//If not visible, enabled our navMeshCountDown (This only allows the NavMesh component to be enabled when 15 seconds have passed)												
			navMeshCountDownTimer += Time.deltaTime;		//This is to avoid the NavMesh from being disabled from simply looking away from the AI for a second or two
			
			if (navMeshCountDownTimer >= 15)
			{
				navMeshAgent.enabled = false;
				audioSource.enabled = false;
				systemOn = false;
				navMeshDisabled = true;

				if (useAnimations)
				{
					anim.Stop();
				}

				navMeshCountDownTimer = 0;
				updateSystemTimer = 0;						//The updateSystemTimer has be ticked, restart.
			}
		}
	}

	void MainSystem ()
	{
		if (navMeshAgent.enabled && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !isFleeing && preyOrPredator == 1 || navMeshAgent.enabled && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !huntMode && preyOrPredator == 2) 
		{
			isGrazing = true;
		}

		if (isFleeing && !isFrozen && !isExhausted && preyOrPredator == 1)
		{
			chaseTimer += Time.deltaTime;
			
			if (chaseTimer >= chaseSeconds)
			{
				isExhausted = true;
				isFleeing = false;
				calculateFlee = false;
				chaseTimer = 0;
			}
		}

		if (isGrazing)
		{
			timer += Time.deltaTime;
			
			if (useAnimations)
			{
				if (grazeAnimationNumber == 1)
				{
					anim.CrossFade(graze1Animation.name);
				}
				
				if (grazeAnimationNumber == 2)
				{
					anim.CrossFade(graze2Animation.name);
				}
				
				if (grazeAnimationNumber == 3)
				{
					anim.CrossFade(graze3Animation.name);
				}
			}

			if (timer >= grazeLength)
			{
				isGrazing = false;
				timer = 0;
				grazeAnimationNumber = Random.Range(1,totalGrazeAnimations+1);

				if (inHerd && animalToFollow != null)
				{
					offSetHerdX = Random.Range(offSetHerdXMin,offSetHerdXMax);
					offSetHerdZ = Random.Range(offSetHerdZMin,offSetHerdZMax);

					NewDestination(new Vector3 (animalToFollow.localPosition.x + offSetHerdX + 5, animalToFollow.localPosition.y, animalToFollow.localPosition.z - offSetHerdZ));
				}

				if (!inHerd || isAlpha == 1)
				{
					Wander();
				}

			}		
		}
	
		if (isFleeing && !rotateFlee && !inHerd && isAlpha == 0 || isFleeing && !rotateFlee && isAlpha == 1)
		{
			rotateTimer += Time.deltaTime;
			
			if (rotateTimer <= 1f && navMeshAgent.enabled)
			{
				Flee();
			}
			
			if (rotateTimer > 1f)
			{
				rotateFlee = true;
			}
		}

		
		if (useAnimations)
		{
			if (anim.IsPlaying(runAnimation.name) && useDustEffect)
			{
				clone.emissionRate = 10;
			}
		}
		
		if (useAnimations)
		{
			if (!anim.IsPlaying(runAnimation.name) && useDustEffect || useDustEffect && waitingForHerd || velocity <= 0.05f && useDustEffect)
			{
				clone.emissionRate = 0;
			}
		}

		if (navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance && !isFleeing && !isCoolingDown && preyOrPredator == 1 || navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance && !huntMode && !isCoolingDown && preyOrPredator == 2)
		{
			if (useAnimations)
			{
				anim.CrossFade(walkAnimation.name);
			}
			
			isGrazing = false;
		}

		if (isFleeing && !isGrazing && systemOn && !isExhausted && velocity > 0.05f && preyOrPredator == 1 || huntMode && !isGrazing && systemOn && !isCoolingDown && velocity > 0.5f && preyOrPredator == 2) 
			{
				if (useAnimations && !withinAttackDistance && !attackWhileRunning)
				{
					anim.CrossFade(runAnimation.name);
					
					if (anim.IsPlaying(runAnimation.name) && useRunSound)
					{
						runTimer += Time.deltaTime;

						if (runTimer >= footStepSeconds && systemOn)
						{
							audioSource.PlayOneShot(runSound);
							runTimer = 0;
						}
					}
				}
				
				navMeshAgent.speed = runSpeed;
			}
	
		
		if (!isFleeing && !isGrazing && !huntMode && !isCoolingDown)
		{
			if (useAnimations)
			{
				anim.CrossFade(walkAnimation.name);
			}
			
			navMeshAgent.speed = walkSpeed;
		}
		
		if (drawPaths)
		{
			NavMeshPath path = new NavMeshPath();
			navMeshAgent.CalculatePath(currentWaypoint.transform.position, path);
			GetPath();
		}
		
		if (startFleeTimer && threatIsOutOfTigger)
		{
			fleeTimer -= Time.deltaTime;
			
			if (fleeTimer <= 0)
			{
				isFleeing = false;
				calculateFlee = false;
				currentlyBeingPursued = false;
				threatIsOutOfTigger = false;
				startFleeTimer = false;
				distantFlee = false;
			}
		}
		
		if (startHuntTimer && !withinAttackDistance)
		{
			huntTimer -= Time.deltaTime;
			
			if (huntTimer <= 0)
			{
				if (isAlpha == 0 && !inHerd || isAlpha == 1)
				{
					ReturnBackToStartingPoint();
				}

				if (isAlpha == 0 && inHerd)
				{
					huntMode = false;
					preySizeMatched = false;
					startHuntTimer = false;
					isCoolingDown = true;
					huntTimer = huntSeconds;
					FollowAlpha();
				}
			}
		}
		
		//This is a cool down system that happens after our animal has reached its max chase seconds and unsuccessfully caught its prey
		if (isCoolingDown)
		{
			coolDownTimer += Time.deltaTime;
			navMeshAgent.speed = walkSpeed;

			if (useAnimations)
			{
				anim.CrossFade(walkAnimation.name);
			}
			
			if (coolDownTimer >= coolDownSeconds)
			{
				isCoolingDown = false;
				coolDownTimer = 0;
			}
		}

		//This is a cool down system that happens after our animal has reached its max chase seconds and unsuccessfully escaped its predator
		if (isExhausted)
		{
			coolDownTimer += Time.deltaTime;

			if (inHerd && isAlpha == 0)
			{
				NewDestination(new Vector3 (animalToFollow.localPosition.x + offSetHerdX + 5, animalToFollow.localPosition.y, animalToFollow.localPosition.z - offSetHerdZ));
			}

			if (useAnimations)
			{
				anim.CrossFade(walkAnimation.name);
			}

			navMeshAgent.speed = walkSpeed;
			
			if (coolDownTimer >= coolDownSeconds)
			{
				isExhausted = false;
				coolDownTimer = 0;
			}
		}
		
		if (huntMode && currentAnimal == null)			//If huntMode is enabled, but our target is lost or killed, set to false and call the Wander function
		{
			huntMode = false;
			withinAttackDistance = false;
			attackWhileRunning = false;
			startHuntTimer = false;
			navMeshAgent.speed = walkSpeed;

			if (!inHerd || isAlpha == 1)
			{
				Wander();
			}

			if (inHerd && animalToFollow != null && navMeshAgent.enabled)
			{
				FollowAlpha();
			}
		}

		if (huntMode && currentAnimal != null && !isCoolingDown)
		{
			if (useAnimations && !withinAttackDistance && velocity > 0.5f)
			{
				anim.CrossFade(runAnimation.name);
			}
			
			if (!withinAttackDistance)
			{
				//Offset our attack position so we don't use the same position as our target's
				navMeshAgent.SetDestination(new Vector3 (currentAnimal.transform.position.x + offSetPosition, currentAnimal.transform.position.y, currentAnimal.transform.position.z + stoppingDistance)); 
			}
			
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				withinAttackDistance = true;
			}
			
			
			if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
			{
				withinAttackDistance = false;
			}
			
			//Attack while running
			if (attackWhileRunning && withinAttackDistance)
			{
				AttackWhileRunning();
			}	
			
			//Attack while stationary
			if (withinAttackDistance && !attackWhileRunning && currentAnimal != null && withinAttackDistance) 
			{
				AttackWhileStationary();
			}
			
		}

		//What happens when our animal's health reaches 0
		if (currentHealth <= 0)
		{
			navMeshAgent.enabled = false;

			if (isAlpha == 1 && herdList.Count >= 1 && !isDead)
			{
				herdNumber = Random.Range(0, herdList.Count);						//If our alpha dies, and it's in a herd, assign alpha status to random memeber in herd and remove alpha from List.

				if (herdList[herdNumber].GetComponent<Emerald_Animal_AI>() != null)
				{
					herdList[herdNumber].GetComponent<Emerald_Animal_AI>().isAlpha = 1;
					herdList[herdNumber].GetComponent<Emerald_Animal_AI>().herdList = herdList;
					herdList[herdNumber].GetComponent<Emerald_Animal_AI>().AssignAlphaStatus();

					herdList.Remove(herdList[herdNumber]);
				}

				isDead = true;
			}

			Instantiate(deadObject, transform.position, transform.rotation);	//If the AI is out of health, instantiate its deadObject replacement.
			Destroy(this.gameObject);											//Destroy the current AI to instaniate the deadObject replacement.
		}
		
		if (damageTaken)
		{
			damageTaken = false;		//Helps keep our damage call from going more than once.
		}

		if (huntMode && navMeshAgent != null && navMeshAgent.enabled)
		{
			navMeshAgent.SetDestination(new Vector3 (currentAnimal.transform.position.x, currentAnimal.transform.position.y, currentAnimal.transform.position.z + stoppingDistance));

			HuntMode();			//Call the HuntMode function, if huntMode is enabled.
		}
		
		if (isFrozen)
		{
			Frozen();			//Call the Frozen function, if isFrozen is enabled and all conditions are met
		}

		if (animalToFollow == null)
		{
			inHerd = false;
		}
	}

	void Update () 
	{
														//Checks to see if our AI are still in view every few seconds.
		SystemOptimizerUpdater ();						//If they are no longer in view, according to Unity's LOD System, disable all components until back in view.
														//Using the NavMesh timer, check to see that our AI are off screen for at least 15 seconds before deactivating.
														//If the AI are out of view, set systemOn to false, which stops Emerald from calculating


		//If systemOn is true, run the Main Emerald System
		if (systemOn)
		{
			MainSystem ();
			CheckSystem();

			if (alignAI)
			{
				AlignNavMesh();
			}

			if (calculateFlee && fleeTarget != null && isAlpha == 1 || calculateFlee && fleeTarget != null && !inHerd)
			{
				if (navMeshAgent.enabled)
				{
					Flee();
				}
			}

			if (inHerd && isAlpha == 0 && animalToFollow != null && navMeshAgent != null && isFleeing || inHerd && isAlpha == 0 && animalToFollow != null && navMeshAgent != null && preyOrPredator == 2 && !huntMode)
			{
				if (navMeshAgent.enabled)
				{
					FollowAlpha();
				}
			}

			if (herdList.Count == maxPackSize)
			{
				herdFull = true;
			}
		}
	}

	//If an alpha is assigned, follow it with various offsets so that each animal has their own space
	public void FollowAlpha ()
	{
			NewDestination(new Vector3 (animalToFollow.localPosition.x + offSetHerdX + 5, animalToFollow.localPosition.y, animalToFollow.localPosition.z - offSetHerdZ));
			
			if (useAnimations && !withinAttackDistance)
			{
				velocity = navMeshAgent.velocity.sqrMagnitude;
				
				if (velocity <= 0.05f && anim.IsPlaying(runAnimation.name))
				{
					anim.CrossFade(idleAnimation.name);
				}
			}
			
			if (offSetHerdX >= -1 && offSetHerdX <= 1)
			{
				offSetHerdX = Random.Range(offSetHerdXMin,offSetHerdXMax);
			}
			
			if (offSetHerdZ >= -1 && offSetHerdZ <= 1)
			{
				offSetHerdZ = Random.Range(offSetHerdZMin,offSetHerdZMax);
			}
			
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				NewDestination(new Vector3 (animalToFollow.localPosition.x + offSetHerdX + 5, animalToFollow.localPosition.y, animalToFollow.localPosition.z - offSetHerdZ));
			}
	}

	//Calculates our aniaml's alignment to the terrain
	public void AlignNavMesh ()
	{
		Vector3 normal = CalculateRotation();
		Vector3 direction = navMeshAgent.steeringTarget - transform.position;
		direction.y = 0.0f;

		if(direction.magnitude > 0.1f && normal.magnitude > 0.1f) 
		{
			Quaternion quaternionLook = Quaternion.LookRotation(direction, Vector3.up);
			Quaternion quaternionNormal = Quaternion.FromToRotation(Vector3.up, normal);
			originalLookRotation = quaternionNormal * quaternionLook;
		}

		transform.rotation = Quaternion.Slerp(transform.rotation, originalLookRotation, Time.deltaTime/0.6f);
	}

	Vector3 CalculateRotation () 
	{
		Vector3 terrainLocalPos = transform.position - terrain.transform.position;
		Vector2 normalizedPos = new Vector2(terrainLocalPos.x / terrain.terrainData.size.x, terrainLocalPos.z / terrain.terrainData.size.z);
		return terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
	}

	public void CheckSystem()
	{
		//For some reason, if our trigger to start our flee timer was missed, set it to true
		if (threatIsOutOfTigger && isFleeing && !startFleeTimer)
		{
			startFleeTimer = true;
		}

		if (alphaWaitForHerd && isFleeing && isAlpha == 1 && hasPack && herdList[0] != null)
		{
			float distance = Vector3.Distance (this.transform.position, herdList[0].transform.position);

			if (distance >= maxDistanceFromHerd)
			{
				navMeshAgent.speed = 0;
				waitingForHerd = true;
			}

			if (distance < maxDistanceFromHerd)
			{
				navMeshAgent.speed = runSpeed;
				waitingForHerd = false;
			}
		}
	}

	public void AssignAlphaStatus ()
	{
		foreach (GameObject G in herdList) 
		{
			if (G != null)
			{
				G.GetComponent<Emerald_Animal_AI>().animalToFollow = gameObject.transform;
			}
		}
	}

	//Frozen handles the freeze system for when a prey stops for a random amount of
	//seconds to simulate a stunned or cautious action. Each time this is trigger, it is recalculated
	public void Frozen ()
	{
		freezeSecondsTimer += Time.deltaTime;

		if (freezeSecondsTimer >= freezeSecondsTotal && navMeshAgent.enabled)
		{
			isFleeing = true;
			currentlyBeingPursued = true;
			Flee();
			freezeSecondsTimer = 0;
			isFrozen = false;
		}
	}


	//Call this function if you want your AI to return back to its starting position
	public void ReturnBackToStartingPoint ()
	{
		NewDestination(startPosition);
		huntMode = false;
		currentlyBeingPursued = false;
		startHuntTimer = false;
		isCoolingDown = true;
		preySizeMatched = false;
		navMeshAgent.speed = runSpeed;
		huntTimer = huntSeconds;

		if (useAnimations)
		{
			anim.CrossFade(runAnimation.name);
		}

		huntMode = false;
	}


	//Call this function if you want to damage this animal from an external script
	public void Damage (float damageReceived)
	{
		currentHealth -= damageReceived;
		audioSource.pitch = Random.Range(minSoundPitch, maxSoundPitch);

		if (useHitSound)
		{
			audioSource.PlayOneShot(getHitSound);
		}

		damageTaken = true;

		if (aggression == 1)
		{
			//This can be uncommented if you would like animals to flee after being hit, even if they are exhausted
			//isFleeing = true;
		}

		if (aggression == 3)
		{
			isCoolingDown = false;
			navMeshAgent.speed = runSpeed;
			huntMode = true;
			coolDownTimer = 0;
			huntTimer = huntSeconds;
		}
		
		if (anim.IsPlaying(graze1Animation.name) && useAnimations || totalGrazeAnimations == 2 && anim.IsPlaying(graze2Animation.name) && useAnimations || totalGrazeAnimations == 3 && anim.IsPlaying(graze3Animation.name) && useAnimations)
		{
			anim.Play(hitAnimation.name);
		}
	}
	
	//This handles how a predator hunts. It will only stop to attack if its velocity is around a stationary speed.
	//This is to stop the AI from attack while moving.
	void HuntMode ()
	{
		updateSpeedTimer += Time.deltaTime;
		isGrazing = false;

		//If our velocity drops near 0, play our idle animation so the animal isn't running in place
		if (useAnimations && !withinAttackDistance)
		{
			velocity = navMeshAgent.velocity.sqrMagnitude;
			
			if (velocity <= 0.05f && anim.IsPlaying(runAnimation.name))
			{
				anim.CrossFade(idleAnimation.name);
			}
		}
		
		if (updateSpeedTimer >= updateSpeed)
		{
			velocity = navMeshAgent.velocity.sqrMagnitude;
			updateSpeedTimer = 0;
		}
		
		if (velocity >= 0.1f && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)  
		{
			attackWhileRunning = true;
		}
		
		if (velocity < 0.1f || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) 
		{
			attackWhileRunning = false; 
		}

		if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && velocity > 0.05f && useAnimations && !withinAttackDistance) //&& !withinAttackDistance
		{
			anim.CrossFade(runAnimation.name); 
			navMeshAgent.speed = runSpeed; 
		}
	}

	//If our AI is within attacking distance, use this function to update its rotations
	//so that it is always facing the player.
	private void RotateTowards (Transform currentPlayer) 
	{
		Vector3 direction = (currentAnimal.transform.position - transform.position).normalized;
		lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8);

		if (drawWaypoints)
		{
			currentWaypoint.transform.position = currentAnimal.transform.position;
		}
	}
	
	void ApplyAttackAnimations()
	{
		if (preyOrPredator == 2)
		{
			if (currentAttackAnimation == 1)
			{
				currentAttackAnimationClip = attackAnimation1;
			}
			
			if (currentAttackAnimation == 2)
			{
				currentAttackAnimationClip = attackAnimation2;
			}
			
			if (currentAttackAnimation == 3)
			{
				currentAttackAnimationClip = attackAnimation3;
			}
			
			if (currentAttackAnimation == 4)
			{
				currentAttackAnimationClip = attackAnimation4;
			}
			
			if (currentAttackAnimation == 5)
			{
				currentAttackAnimationClip = attackAnimation5;
			}
			
			if (currentAttackAnimation == 6)
			{
				currentAttackAnimationClip = attackAnimation6;
			}
		}
	}

	void AttackWhileStationary ()
	{
		if (useAnimations)
		{
			if (currentAnimal != null && withinAttackDistance)
			{
				RotateTowards(currentAnimal.transform);
			}
		}

		if (!useAnimations)
		{
			if (currentAnimal != null && withinAttackDistance)
			{
				RotateTowards(currentAnimal.transform);
			}
		}

		if (drawWaypoints)
		{
			currentWaypoint.transform.position = currentAnimal.transform.position;
		}
		
		attackTimer += Time.deltaTime;
		
		if (attackTimer >= attackTime)
		{
			if (useAnimations)
			{
				if (!attackWhileRunning)
				{
					if (currentAttackAnimation == 1)
					{
						currentAttackAnimationClip= attackAnimation1;
						anim.CrossFade(attackAnimation1.name);
					}
					
					if (currentAttackAnimation == 2)
					{
						
						currentAttackAnimationClip = attackAnimation2;
						anim.CrossFade(attackAnimation2.name);
					}
					
					if (currentAttackAnimation == 3)
					{
						
						currentAttackAnimationClip = attackAnimation3;
						anim.CrossFade(attackAnimation3.name);
					}
					
					if (currentAttackAnimation == 4)
					{
						
						currentAttackAnimationClip = attackAnimation4;
						anim.CrossFade(attackAnimation4.name);
					}
					
					if (currentAttackAnimation == 5)
					{
						
						currentAttackAnimationClip = attackAnimation5;
						anim.CrossFade(attackAnimation5.name);
					}
					
					if (currentAttackAnimation == 6)
					{
						
						currentAttackAnimationClip = attackAnimation6;
						anim.CrossFade(attackAnimation6.name);
					}
					
					if (!damageDealt)
					{
						if (useAttackSound)
						{
							audioSource.PlayOneShot(attackSound);
						}

						//Attack Prey
						if (currentAnimal.gameObject.tag == preyTagName)
						{
							currentAnimal.GetComponent<Emerald_Animal_AI>().currentHealth -= attackDamage;
						}

						//Attack Player
						if (currentAnimal.gameObject.tag == playerTagName && aggression == 3)
						{
							currentAnimal.GetComponent<PlayerHealth>().DamagePlayer(attackDamage);

							//Uncomment if you would like to add a hit effect (Ex: UFPS shake effect)
							//Instantiate(hitEffect, this.transform.position, this.transform.rotation);
						}
						
						damageDealt = true;
						
					}
				}
			}

			if (!useAnimations)
			{
				if (!attackWhileRunning)
				{
					if (!damageDealt)
					{
						if (useAttackSound)
						{
							audioSource.PlayOneShot(attackSound);
						}
						
						//Attack Prey
						if (currentAnimal.gameObject.tag == preyTagName)
						{
							currentAnimal.GetComponent<Emerald_Animal_AI>().currentHealth -= attackDamage;
						}
						
						//Attack Player
						if (currentAnimal.gameObject.tag == playerTagName && aggression == 3)
						{
							currentAnimal.GetComponent<PlayerHealth>().DamagePlayer(attackDamage);

							//Uncomment if you would like to add a hit effect (Ex: UFPS shake effect)
							//Instantiate(hitEffect, this.transform.position, this.transform.rotation);
						}
						
						damageDealt = true;
						
					}
				}
			}
		}
		
		if (useAnimations && attackTimer >= attackTime + currentAttackAnimationClip.length)
		{
			anim.CrossFade(idleBattleAnimation.name);
			currentAttackAnimation = Random.Range(1, totalAttackAnimations);
			withinAttackDistance = false;
			damageDealt = false;
			attackTimer = 0;
		}

		if (!useAnimations && attackTimer >= attackTime)
		{
			currentAttackAnimation = Random.Range(1, totalAttackAnimations);
			withinAttackDistance = false;
			damageDealt = false;
			attackTimer = 0;
		}
	}



	void AttackWhileRunning ()
	{
		attackTimer += Time.deltaTime;

		if (drawWaypoints)
		{
			currentWaypoint.transform.position = currentAnimal.transform.position;
		}
		
		if (currentAnimal != null)
		{
			navMeshAgent.SetDestination(new Vector3 (currentAnimal.transform.position.x, currentAnimal.transform.position.y, currentAnimal.transform.position.z + stoppingDistance));
		}
		
		if (attackTimer >= attackTime) 
		{		
			if (!damageDealt)
			{
				if (useAttackSound)
				{
					audioSource.PlayOneShot(attackSound);
				}

				//Attack Prey
				if (currentAnimal.gameObject.tag == preyTagName)
				{
					currentAnimal.GetComponent<Emerald_Animal_AI>().currentHealth -= attackDamage;
				}

				//Attack Player
				if (currentAnimal.gameObject.tag == playerTagName && aggression == 3)
				{
					currentAnimal.GetComponent<PlayerHealth>().DamagePlayer(attackDamage);

					//Uncomment if you would like to add a hit effect (Ex: UFPS shake effect)
					//Instantiate(hitEffect, this.transform.position, this.transform.rotation);
				}
				
				if (useRunAttackAnimations)
				{
					anim.Play(runAttackAnimation.name);
				}
				
				damageDealt = true;
			}
			
			if (attackTimer > attackTime)
			{
				attackTimer = 0;
				damageDealt = false;
			}
		}
	}

	
	void OnTriggerEnter(Collider other) 
	{
		//If the triggered object is our player, and the animal is cowardly, flee.
		if (other.gameObject.tag == playerTagName && aggression == 1 && !isExhausted)
		{
			freezeSecondsTotal = Random.Range(freezeSecondsMin, freezeSecondsMax);
			isFrozen = true;
			fleeTimer = extraFleeSeconds;
			threatIsOutOfTigger = false;
			fleeTarget = other.gameObject;

			//Checks to make sure tired animals aren't fleeing
			if (!isExhausted)
			{
				calculateFlee = true;
			}

			//If our animal is in a heard and a predator approaches, alert the leader to flee as well
			if (inHerd && isAlpha == 0 && animalToFollow != null && preyOrPredator == 1)
			{
				animalToFollow.GetComponent<Emerald_Animal_AI>().fleeTarget = fleeTarget;
				animalToFollow.GetComponent<Emerald_Animal_AI>().isFleeing = true;
				animalToFollow.GetComponent<Emerald_Animal_AI>().calculateFlee = true;
				animalToFollow.GetComponent<Emerald_Animal_AI>().distantFlee = true; 
			}

			if (playSoundOnFlee)
			{
				audioSource.PlayOneShot(fleeSound);
			}

			if (isAlpha == 1 && preyOrPredator == 1)
			{
				foreach (GameObject G in herdList) 
				{
					if (G != null)
					{
						if (!G.GetComponent<Emerald_Animal_AI>().isExhausted)
						{
							G.GetComponent<Emerald_Animal_AI>().isFleeing = true;
						}
					}
				}
			}
		}

		//Flee from predator
		//If the triggered object is a predator, and the animal is cowardly, call the frozen function (this causes a few seconds of randomized delay)
		if (other.gameObject.tag == predatorTagName && preyOrPredator == 1 && other.gameObject.GetComponent<Emerald_Animal_AI>().predatorSize == preySize && aggression == 1 && !isExhausted)
		{
			freezeSecondsTotal = Random.Range(freezeSecondsMin, freezeSecondsMax);
			isFrozen = true;
			fleeTimer = extraFleeSeconds;
			threatIsOutOfTigger = false;
			preySizeMatched = true;

			if (other.gameObject.GetComponent<Emerald_Animal_AI>().inHerd)
			{
				fleeTarget = other.gameObject.GetComponent<Emerald_Animal_AI>().animalToFollow.gameObject;
			}

			if (other.gameObject.GetComponent<Emerald_Animal_AI>().inHerd == false)
			{
				fleeTarget = other.gameObject;
			}

			//Checks to make sure tired animals aren't fleeing
			if (!isExhausted)
			{
				calculateFlee = true;
			}

			//If our animal is in a heard and an predator approaches, alert the leader to flee as well
			if (inHerd && isAlpha == 0 && preyOrPredator == 1 && animalToFollow != null)
			{
				animalToFollow.GetComponent<Emerald_Animal_AI>().fleeTarget = other.gameObject; 
				animalToFollow.GetComponent<Emerald_Animal_AI>().isFleeing = true;
				animalToFollow.GetComponent<Emerald_Animal_AI>().calculateFlee = true;
				animalToFollow.GetComponent<Emerald_Animal_AI>().distantFlee = true; 
			}
		}

		//Chase Prey
		//If the triggered object is a prey, and the animal is a proper prey size, enabled huntMode and chase target
		if (other.gameObject.tag == preyTagName && aggression == 3 && !huntMode && other.gameObject.GetComponent<Emerald_Animal_AI>().preySize == predatorSize && other.gameObject.GetComponent<Emerald_Animal_AI>().currentlyBeingPursued == false && !isCoolingDown)
		{
			currentAnimal = other.gameObject;
			navMeshAgent.speed = runSpeed;
			offSetPosition = Random.Range(-5,5);
			offSetDistance = Random.Range(-7,3) + 2;

			huntMode = true;
			startHuntTimer = true;

			//If our animal is in a heard and an prey approaches, alert the leader to hunt as well
			if (inHerd && isAlpha == 0 && preyOrPredator == 2 && animalToFollow != null)
			{
				animalToFollow.GetComponent<Emerald_Animal_AI>().currentAnimal = currentAnimal;
				animalToFollow.GetComponent<Emerald_Animal_AI>().huntMode = true;
				animalToFollow.GetComponent<Emerald_Animal_AI>().startHuntTimer = true;
				//animalToFollow.GetComponent<Emerald_Animal_AI>().attackingEnabled = false;
			}
		}

		//Chase Player
		//If the triggered object is our player, and the animal's prey size is large, enabled huntMode and chase target
		if (other.gameObject.tag == playerTagName && aggression == 3 && predatorSize == 3)
		{
			currentAnimal = other.gameObject;

			if (!isCoolingDown)
			{
				navMeshAgent.speed = runSpeed;
				offSetPosition = Random.Range(-5,5);
				offSetDistance = Random.Range(-7,3) + 2;
				
				huntMode = true;
				startHuntTimer = true;

				//If our animal is in a heard and an prey approaches, alert the leader to hunt as well
				if (inHerd && isAlpha == 0 && preyOrPredator == 2 && animalToFollow != null)
				{
					animalToFollow.GetComponent<Emerald_Animal_AI>().currentAnimal = currentAnimal;
					animalToFollow.GetComponent<Emerald_Animal_AI>().huntMode = true;
					animalToFollow.GetComponent<Emerald_Animal_AI>().startHuntTimer = true;
				}
			}
		}

		//Generates our herd system
		//If the other gameobject is the same animal and they are an alpha, follow heard
		if (other.gameObject.GetComponent<Emerald_Animal_AI>() != null && !inHerd && isAlpha == 0 && other.gameObject.GetComponent<Emerald_Animal_AI>().isAlpha == 1 && other.gameObject.GetComponent<Emerald_Animal_AI>().animalNameType == animalNameType && other.gameObject.GetComponent<Emerald_Animal_AI>().herdFull == false)
		{
			if (other.gameObject.GetComponent<Emerald_Animal_AI>().herdList.Count <= other.gameObject.GetComponent<Emerald_Animal_AI>().maxPackSize)
			{
				animalToFollow = other.gameObject.transform;
				inHerd = true;
			}

			if (other.gameObject.GetComponent<Emerald_Animal_AI>().packCreated == false)
			{
				//Add (Alpha) to the alpha's name
				other.gameObject.name = other.gameObject.name + " (Alpha)";
				other.gameObject.GetComponent<Emerald_Animal_AI>().packCreated = true;
			}
		}

		//Assign List of memebers of herd for alpha
		if (other.gameObject.GetComponent<Emerald_Animal_AI>() != null && other.gameObject.GetComponent<Emerald_Animal_AI>().animalToFollow == this.gameObject.transform && other.gameObject.GetComponent<Emerald_Animal_AI>().markInPack == false && isAlpha == 1 && other.gameObject.GetComponent<Emerald_Animal_AI>().isAlpha == 0)
		{
			//Limits the number of animals that can be in a pack to what's set with the maxPackSize
			if (herdList.Count <= maxPackSize && !herdFull)
			{
				other.gameObject.GetComponent<Emerald_Animal_AI>().markInPack = true;
				hasPack = true;
				herdList.Add(other.gameObject);
			}
		}
	}
	
	void OnTriggerExit(Collider other) 
	{
		if (other.gameObject.tag == playerTagName && aggression == 1)
		{
			startFleeTimer = true;
			threatIsOutOfTigger = true;
		}

		if (other.gameObject.tag == predatorTagName && aggression == 1 && other.gameObject.GetComponent<Emerald_Animal_AI>().predatorSize == preySize)
		{
			startFleeTimer = true;
			threatIsOutOfTigger = true;
		}
	}
}

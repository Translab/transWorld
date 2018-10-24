using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Hand))]
public class XRTK_ControllerFly : MonoBehaviour {
	
	Hand handComponent;
	SteamVR_Input_Sources handType;
	private Player player;

	[Header("Basics")]
	[SteamVR_DefaultAction("Squeeze")]
	public SteamVR_Action_Single triggerValue;
	private float triggerPressure = 0.0f; // o link with triggerValue
	private bool flying = false; // to link with triggerState

	//flying param
	public float flySpeed = 1.0f; //fly speed by default 1.0
	[Range(1,6)]
	public int speedLogFactor = 2;

	private Vector3 facing_direction; //just for reference, showing where your pointing object is facing
	private Vector3 flyVelocity;
	private Vector3 flyAcceleration;
	private float flyAccelerationMax = 0.2f;
	private float speedCompensation = 0.1f;
	[Header("Body Collision")]
	public bool collisionDetection = true;

	private XRTK_BodyCollider bodyCollider;
	private Vector3 bodyPositionColliding; //temporal vector3 to record the body position of the colliding moment
	private bool departureFromLanding = false; 

	[Header("Gravity Options")]
	//landing height
	[Tooltip("true if you need gravity, it will let you fall onto objects")]
	public bool gravity = true; //check this if you want gravity to return back to the middle plane / terrain
	
	[Tooltip("a landing height is a height that you will return to, even if there is no actual terrain, typically 0 or mataches with the terrain / plane height")]
	public float YlevelofTerrain = 0.0f; //typically 0 or matches with terrain / plane height
	[Tooltip("determines how much your gravity acceleration is, if too high, you may not land properly")]
	[Range(0,1)] public float gravityFactor = 0.05f; 
	[Tooltip("tolerence of landing detection, can be less if your gravity isn't very strong")]
	[Range(0.01f, 1)]public float landingThreshold = 0.05f; //tolerence of landing detection, can be much less if your gravity factor isn't very high

	private Vector3 landingPointReference;
	private float landingDistanceReference; //distance between flying body and reference plane
	private Vector3 fallVelocity;
	private Vector3 gravityAcceleration;

	private float gravityAccelerationMax;
	private bool onGround = true;
	private bool onObject = false;

	[Header("Floating Effect")]
	//floating effect 
	[Tooltip("true if you want floating effect while not flying")]
	public bool floatingEffect = true;
	[Tooltip("amplitude of floating, aka Height")]
	public float floatingHeight = 0.003f; //amplitude of floating
	[Tooltip("intensity of floating")]
	[Range(0.001f, 10)]public float floatingIntensity = 1.0f; //floating frequency

	private Vector3 floatingTempPos = new Vector3(0,0,0);

	void Start () {
		handComponent = gameObject.GetComponent<Hand>();
		handType = handComponent.handType;
		
		player = Valve.VR.InteractionSystem.Player.instance;

			if ( player == null )
			{
				Debug.LogError( "Teleport: No Player instance found in map." );
				Destroy( this.gameObject );
				return;
			}

		bodyCollider = FindObjectOfType<XRTK_BodyCollider>();

		//init acceleration 
		gravityAccelerationMax = flySpeed * 0.1f * gravityFactor;
	}
	
	void Update () {
		//update controller actions
		triggerPressure = Mathf.Pow(triggerValue.GetAxis(handType), speedLogFactor);
		
		flying = triggerPressure > 0.001f ? true : false;
		//Debug.Log(flying + "flying");


		facing_direction = XRTK.ControllerStats.getControllerRotation(handComponent) * Vector3.forward;
		landingDistanceReference = Mathf.Abs(player.trackingOriginTransform.position.y - YlevelofTerrain);
		//Debug.Log(landingDistanceReference + " distance");

		//switch on ground status
		if (landingDistanceReference < landingThreshold) {
			onGround = true;
		} else {
			if (flying) { //only when flying, switch onGround to be false
				//this means, if simply floating, then its okay to exceed the landing detection threshold
				onGround = false;
			}
		}
			
		// check collision status
		if (collisionDetection) {
			if (bodyCollider.Colliding) {
				Debug.Log("bodyCollider.Colliding = " + bodyCollider.Colliding);
				onObject = true;
				// bodyPositionColliding = player.trackingOriginTransform.position;
				bodyPositionColliding = bodyCollider.CollisionPoint;

			} else if (!bodyCollider.Colliding) {
				onObject = false;
				departureFromLanding = false;
			}
		} else {
			onObject = false;
		}
		Debug.Log(onObject + " on Object");
		if (flying) {
			//fly function
			flyAcceleration = facing_direction;
			if (flyAcceleration.magnitude > flyAccelerationMax) {
				flyAcceleration = Vector3.ClampMagnitude (flyAcceleration, flyAccelerationMax);
			}
			flyVelocity += flyAcceleration * flySpeed * speedCompensation *triggerPressure;
			

			//collision compensation
			if (onObject) {
				//temporary solution
				//TO DO: improve the calculation
				float angle = Vector3.Angle(flyVelocity, bodyPositionColliding - flyVelocity);
				if (angle < 90){
					flyVelocity = Vector3.Cross(flyVelocity, bodyPositionColliding - flyVelocity) * Mathf.Pow(angle / 90, 4);
				}
				
				//TO DO: gravity test
				if (departureFromLanding) {
					player.trackingOriginTransform.position += flyVelocity;
				}
				//flyVelocity = Vector3.zero;
			} 
			
			player.trackingOriginTransform.position += flyVelocity;

			flyVelocity *= 0.2f; //not zeros but decays velocity, to leave some inertia
			flyAcceleration = Vector3.zero; //zeros acceleration
		} else {
			if (gravity) {
				if (onObject) {
					player.trackingOriginTransform.position = bodyPositionColliding;
					fallVelocity *= 0.0f;
					departureFromLanding = true;
				}
				if (!onGround && !onObject) {
					landingPointReference = new Vector3 (player.trackingOriginTransform.position.x, YlevelofTerrain, player.trackingOriginTransform.position.z);
					gravityAcceleration = landingPointReference -player.trackingOriginTransform.position;
					if (gravityAcceleration.magnitude > gravityAccelerationMax) {
						gravityAcceleration = Vector3.ClampMagnitude (gravityAcceleration, gravityAccelerationMax);
					}
					fallVelocity += gravityAcceleration;

					player.trackingOriginTransform.position += fallVelocity;
					gravityAcceleration = gravityAcceleration * 0.0f; // zeros acceleration
				} else if (onGround && !onObject){
					fallVelocity *= 0.0f;// zeros velocity to stop movement
					if (floatingEffect) {
						floatingTempPos = player.trackingOriginTransform.position;
						floatingTempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * floatingIntensity) * floatingHeight;
						player.trackingOriginTransform.position = floatingTempPos;
					} 
				}
			}
			if (floatingEffect) {
				if (!onObject) {
					floatingTempPos = player.trackingOriginTransform.position;
					floatingTempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * floatingIntensity) * floatingHeight;
					player.trackingOriginTransform.position = floatingTempPos;
			
				}
			}
		}
	
	}
}//XRTK_ControllerFly

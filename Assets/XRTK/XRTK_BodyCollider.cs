using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent( typeof( CapsuleCollider ) )]
public class XRTK_BodyCollider : MonoBehaviour {
		private Transform playerTransform;
		private Player player;
		private CapsuleCollider capsuleCollider;
		public float bodyHeight = 1.6f;
		private float distanceFromFloor = 1.6f;
		public bool Colliding{
			get{
				return colliding;
			}
		}
		private bool colliding;

		//-------------------------------------------------
		void Awake()
		{
			capsuleCollider = GetComponent<CapsuleCollider>();
			
		}
		void Start(){
			player = Valve.VR.InteractionSystem.Player.instance;
		}


		//-------------------------------------------------
		void FixedUpdate()
		{
			playerTransform = player.hmdTransform;
			//float distanceFromFloor = Vector3.Dot( playerTransform.position, Vector3.up );
			distanceFromFloor = bodyHeight;
			Debug.Log(colliding);
			capsuleCollider.height = Mathf.Max( capsuleCollider.radius, distanceFromFloor );
			
			transform.position = playerTransform.position - 1.0f * distanceFromFloor * Vector3.up;
		}
		void OnCollisionEnter(Collision collision){
			if (collision.collider.tag != "Player"){
				Debug.Log("enterrredd");
				colliding = true;
			}
		}
		void OnCollisionExit(Collision collisionInfo){
			if (collisionInfo.collider.tag != "Player"){
				colliding = false;
			}
		}
}

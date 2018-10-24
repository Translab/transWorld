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
		private Vector3 collisionPoint;
		public Vector3 CollisionPoint{
			get{
				return collisionPoint;
			}
		}

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
			capsuleCollider.height = Mathf.Max( capsuleCollider.radius, distanceFromFloor );
			
			transform.position = playerTransform.position - 1.0f * distanceFromFloor * Vector3.up;
		}
		void OnCollisionEnter(Collision collision){
			if (collision.collider.tag != "Player" || collision.collider.tag != "Terrain"){
				//Debug.Log("enterrredd");
				Vector3 sum = Vector3.zero;
				for (int i = 0; i < collision.contacts.Length; i ++){
					sum += collision.contacts[i].point;	
				}
				collisionPoint = sum / collision.contacts.Length;
				colliding = true;
			}
		}
		void OnCollisionExit(Collision collisionInfo){
			if (collisionInfo.collider.tag != "Player" || collisionInfo.collider.tag != "Terrain"){
				colliding = false;
			}
		}
}

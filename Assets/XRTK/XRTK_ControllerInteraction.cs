using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Hand))]
public class XRTK_ControllerInteraction : MonoBehaviour {

	Hand hand;
	private Player player;
	HandEvent handevent;
	// Use this for initialization
	void Start () {
		hand = gameObject.GetComponent<Hand>();

		player = Valve.VR.InteractionSystem.Player.instance;

			if ( player == null )
			{
				Debug.LogError( "Teleport: No Player instance found in map." );
				Destroy( this.gameObject );
				return;
			}

	}
	// Update is called once per frame
	void Update () {
		if (XRTK.ControllerStats.getTrackPadTouch(hand)){
			Debug.Log(hand.handType + " touched at " + XRTK.ControllerStats.getTrackPadPos(hand));
		}	
		
	}
}

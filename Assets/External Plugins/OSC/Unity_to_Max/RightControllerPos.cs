using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightControllerPos : MonoBehaviour {

	// Use this for initialization
	public OSC osc;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		OscMessage message = new OscMessage();

        message.address = "/RightXYZ";
        message.values.Add(transform.position.x);
        message.values.Add(transform.position.y);
        message.values.Add(transform.position.z);
        osc.Send(message);

		message.address = "/RightQuatXYZW";
		message.values.Add(transform.rotation.x);
		message.values.Add(transform.rotation.y);
		message.values.Add(transform.rotation.z);
		message.values.Add(transform.rotation.w);
		osc.Send(message);
		//Debug.Log (transform.rotation.x);
			
	}
}

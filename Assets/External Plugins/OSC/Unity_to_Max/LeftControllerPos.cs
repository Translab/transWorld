using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftControllerPos : MonoBehaviour {

	// Use this for initialization
	public OSC osc;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		 OscMessage message = new OscMessage();

        message.address = "/LeftXYZ";
        message.values.Add(transform.position.x);
        message.values.Add(transform.position.y);
        message.values.Add(transform.position.z);
        osc.Send(message);

        message = new OscMessage();
        message.address = "/LeftX";
        message.values.Add(transform.position.x);
        osc.Send(message);

        message = new OscMessage();
        message.address = "/LeftY";
        message.values.Add(transform.position.y);
        osc.Send(message);

        message = new OscMessage();
        message.address = "/LeftZ";
        message.values.Add(transform.position.z);
        osc.Send(message);
	}
}

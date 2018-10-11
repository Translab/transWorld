using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePosScale : MonoBehaviour {

	// Use this for initialization
	public OSC osc;
	public string handlerNamePos;
	public string handlerNameScale;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		 OscMessage message = new OscMessage();

        message.address = handlerNamePos;
        message.values.Add(transform.position.x);
        message.values.Add(transform.position.y);
        message.values.Add(transform.position.z);
        osc.Send(message);

		message = new OscMessage();
		message.address = handlerNameScale;
		message.values.Add(transform.localScale.x / 2);

		osc.Send(message);


//        message = new OscMessage();
//        message.address = "/HeadX";
//        message.values.Add(transform.position.x);
//        osc.Send(message);
//
//        message = new OscMessage();
//        message.address = "/HeadY";
//        message.values.Add(transform.position.y);
//        osc.Send(message);
//
//        message = new OscMessage();
//        message.address = "/HeadZ";
//        message.values.Add(transform.position.z);
//        osc.Send(message);
	}
}

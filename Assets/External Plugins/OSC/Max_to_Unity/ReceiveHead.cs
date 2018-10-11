using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveHead : MonoBehaviour {

   	public OSC osc;


	// Use this for initialization
	void Start () {
	   osc.SetAddressHandler( "/M_HeadXYZ" , OnReceiveXYZ );
       osc.SetAddressHandler("/M_HeadX", OnReceiveX);
       osc.SetAddressHandler("/M_HeadY", OnReceiveY);
       osc.SetAddressHandler("/M_HeadZ", OnReceiveZ);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnReceiveXYZ(OscMessage message){
		float x = message.GetFloat(0);
         float y = message.GetFloat(1);
		float z = message.GetFloat(2);

		transform.position = new Vector3(x,y,z);
	}

    void OnReceiveX(OscMessage message) {
        float x = message.GetFloat(0);

        Vector3 position = transform.position;

        position.x = x;

        transform.position = position;
    }

    void OnReceiveY(OscMessage message) {
        float y = message.GetFloat(0);

        Vector3 position = transform.position;

        position.y = y;

        transform.position = position;
    }

    void OnReceiveZ(OscMessage message) {
        float z = message.GetFloat(0);

        Vector3 position = transform.position;

        position.z = z;

        transform.position = position;
    }
}

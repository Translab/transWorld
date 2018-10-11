using UnityEngine;
using System.Collections;

public class SphereQuatControl : MonoBehaviour {
    
   	public OSC osc;
	public string handlerName;
    private Quaternion sphereQuat;

	// Use this for initialization
	void Start () {
	   osc.SetAddressHandler( handlerName , FSphereQuatXYZW );
    
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void FSphereQuatXYZW(OscMessage message){
		float x = message.GetFloat(0);
         float y = message.GetFloat(1);
		float z = message.GetFloat(2);
        float w = message.GetFloat(3);

        sphereQuat = new Quaternion(x, y, z, w);
		transform.rotation = sphereQuat;
	}

}

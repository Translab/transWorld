using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUpdated : MonoBehaviour {

	public float moveSpeed;
	// speed of movement

	public bool _translate = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Translate(-Vector3.forward* moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate (new Vector3 (0, 1, 0) * Time.deltaTime * moveSpeed * 10, Space.World);
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate (new Vector3 (0, -1, 0) * Time.deltaTime * moveSpeed * 10, Space.World);
		} 
	}
}

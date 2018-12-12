using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFloor : MonoBehaviour {

    public Material mat;
    //public float frequency;
    //public float amplitude;

    public int resolution;

    //private int t;

	// Use this for initialization
	void Start () {
        //t = 0;
        //resolution = 10;
	}

    // Update is called once per frame
    void Update () {

        float step = 2f / resolution;

        //get time (do inside shader)
        //float t = Time.time;

        //step - related to resolution to maintain 
        mat.SetFloat("_Step", step);

        //mat.SetFloat("_Frequency", frequency);
        //mat.SetFloat("_Amplitude", amplitude);
        //mat.SetFloat("_WaveTime", t);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {
    public Transform eye;
    public Material _material;
    public Material _material_1;
    public Material _material_2;
    public Material _material_3;
    public Material _material_4;
    public Material _material_5;
    public Material _material_6;
    public Material _material_7;
    public Material _material_8;
    public Material _material_9;
    public Material _material_10;
    public Material _material_11;
    public Material _material_12;


    // Use this for initialization
    void Start () {
        //_material = GetComponent<Material>();
    }
	
	// Update is called once per frame
	void Update () {
        _material.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_1.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_2.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_3.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_4.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_5.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_6.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_7.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_8.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_9.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_10.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_11.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));
        _material_12.SetVector("_FacePos", new Vector4(eye.position.x , eye.position.y, eye.position.z, (float)0.0));

	}
}

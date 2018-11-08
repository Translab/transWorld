using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Mathf;


public class Tesselate : MonoBehaviour {

    public GameObject cube;
    public int numObjects;
    public float objScale;
    private GameObject[] cubeHolder;

    private float xOffset;
    private float yOffset;
    private float zOffset;
    private int n;
    private float increment;

	// Use this for initialization
	void Start () {
        xOffset = -(numObjects / 2) * objScale;
        yOffset = -(numObjects / 2) * objScale;
        zOffset = -(numObjects / 2) * objScale;
        int arraySize = numObjects * numObjects * numObjects;
        cubeHolder = new GameObject[arraySize];
        increment = objScale; //might be incorrect
        n = 0;
        for (int i=1; i< numObjects; i++)
        {
            zOffset = -(numObjects / 2) * objScale;
            yOffset = -(numObjects / 2) * objScale;
            for (int j=1; j<numObjects; j++)
            {
                zOffset = -(numObjects / 2) * objScale;
                for (int k=1; k<numObjects; k++)
                {
                    cubeHolder[n] = (GameObject)Instantiate(cube, gameObject.transform);
                    cubeHolder[n].transform.localScale = new Vector3(objScale, objScale, objScale);
                    cubeHolder[n].transform.position = new Vector3(xOffset, yOffset, zOffset);
                    //instantiate
                    //GameObject
                    n = n + 1;
                    zOffset = zOffset + increment;
                }
                yOffset = yOffset + increment;
            }
            xOffset = xOffset + increment;
        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

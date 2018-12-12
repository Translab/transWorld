using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateIncrementPositions : MonoBehaviour {

    Transform t;

    private void Awake()
    {
        t = gameObject.GetComponent<Transform>();
    }

    //return an array of Vector3 points describing locations of objects on ring
    public Vector3[] GenerateElementPositions(int increment, int maxAngle, Vector3 rotAxis)
    {
        //t.rotation.Set(0, 0, 0, 0); //reset rotation
        int numElements = maxAngle / increment;
        Vector3[] positions = new Vector3[numElements];

        for (int i = 0; i < (maxAngle / increment); i++)
        {
            Debug.Log(t.rotation);
            t.Rotate(rotAxis * increment);
            Vector3 point = t.forward;
            //Vector3 point = t.rotation * new Vector3(0, 0, 1);
            //Debug.Log(t.rotation);

            positions[i] = point;
        }
        //t.rotation.Set(0, 0, 0, 0); //reset rotation
        return positions;
    }

}

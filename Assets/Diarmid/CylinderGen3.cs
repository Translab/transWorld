using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGen3 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {


        
        CreateCylinderBetweenPoints(new Vector3(11, 13, 17), new Vector3(13, 17, 19), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(13, 17, 19), new Vector3(17, 19, 23), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(17, 19, 23), new Vector3(19, 23, 29), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(19, 23, 29), new Vector3(23, 29, 31), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(23, 29, 31), new Vector3(29, 31, 37), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(29, 31, 37), new Vector3(31, 37, 41), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(31, 37, 41), new Vector3(37, 41, 43), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(37, 41, 43), new Vector3(41, 43, 47), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(41, 43, 47), new Vector3(43, 47, 53), 0.1f);

        transform.eulerAngles = new Vector3(0, 144, 0);
       // transform.localPosition = new Vector3(0, 0, 170);

       // transform.localPosition = new Vector3(0, 0, 110);

    }

    void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
    {
        GameObject group = GameObject.Find("CylinderGen_3");

        Vector3 offset = (end - start) / 2.0f;

        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // Position it
        cylinder.transform.position = offset + start;
        cylinder.transform.localScale = new Vector3(width * start.x, offset.magnitude * 73 / end.z, width * start.x);
        cylinder.GetComponent<Renderer>().material.color = new Color(1 / (start.x / 11), 1 / (start.x /11), 1 / (start.x / 11), 1);
        // Rotate it
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);

        cylinder.transform.parent = group.transform;
    }

}


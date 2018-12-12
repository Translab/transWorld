using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGen5 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        CreateCylinderBetweenPoints(new Vector3(23, 29, 31), new Vector3(29, 31, 37), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(29, 31, 37), new Vector3(31, 37, 41), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(31, 37, 41), new Vector3(37, 41, 43), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(37, 41, 43), new Vector3(41, 43, 47), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(41, 43, 47), new Vector3(43, 47, 53), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(43, 47, 53), new Vector3(47, 53, 59), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(47, 53, 59), new Vector3(53, 59, 61), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(53, 59, 61), new Vector3(59, 61, 67), 0.1f);
        CreateCylinderBetweenPoints(new Vector3(59, 61, 67), new Vector3(61, 67, 71), 0.1f);

        transform.eulerAngles = new Vector3(0, 288, 0);
       // transform.localPosition = new Vector3(0, 0, 310);

       //  transform.localPosition = new Vector3(0, 0, 230);

    }

    void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
    {
        GameObject group = GameObject.Find("CylinderGen_5");

        Vector3 offset = (end - start) / 2.0f;

        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // Position it
        cylinder.transform.position = offset + start;
        cylinder.transform.localScale = new Vector3(width * start.x, offset.magnitude * 73 / end.z, width * start.x);
        cylinder.GetComponent<Renderer>().material.color = new Color(1 / (start.x / 23), 1 / (start.x / 23), 1 / (start.x / 23), 1);
        // Rotate it
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);

        cylinder.transform.parent = group.transform;
    }

}




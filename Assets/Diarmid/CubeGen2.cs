﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGen2 : MonoBehaviour
{

    // Use this for initialization

    void Start()
    {



        CreateCubeBetweenPoints(new Vector3(7, 11, 13), new Vector3(11, 13, 17), 0.1f);
        CreateCubeBetweenPoints(new Vector3(11, 13, 17), new Vector3(13, 17, 19), 0.1f);
        CreateCubeBetweenPoints(new Vector3(13, 17, 19), new Vector3(17, 19, 23), 0.1f);
        CreateCubeBetweenPoints(new Vector3(17, 19, 23), new Vector3(19, 23, 29), 0.1f);
        CreateCubeBetweenPoints(new Vector3(19, 23, 29), new Vector3(23, 29, 31), 0.1f);
        CreateCubeBetweenPoints(new Vector3(23, 29, 31), new Vector3(29, 31, 37), 0.1f);
        CreateCubeBetweenPoints(new Vector3(29, 31, 37), new Vector3(31, 37, 41), 0.1f);
        CreateCubeBetweenPoints(new Vector3(31, 37, 41), new Vector3(37, 41, 43), 0.1f);
        CreateCubeBetweenPoints(new Vector3(37, 41, 43), new Vector3(41, 43, 47), 0.1f);

        transform.eulerAngles = new Vector3(0, 108, 0);
       // transform.localPosition = new Vector3(0, 0, 130);

       // transform.localPosition = new Vector3(0, 0, 70);

    }

    void CreateCubeBetweenPoints(Vector3 start, Vector3 end, float width)
    {
        GameObject group = GameObject.Find("CubeGen_2");

        Vector3 offset = (end - start) / 2.0f;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Position it
        cube.transform.position = offset + start;
        cube.transform.localScale = new Vector3(width * start.x, offset.magnitude * 73 / end.z, width * start.x);
        cube.GetComponent<Renderer>().material.color = new Color(1 / (start.x / 7), 1 / (start.x / 7), 1 / (start.x / 7), 1);
        // Scale it

        // Rotate it
        cube.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
        cube.transform.parent = group.transform;
    }
}

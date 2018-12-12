using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class NewGen_7 : MonoBehaviour
{

    public TextAsset textFile;
    public float[] numbers;

    void Start()
    {

        // Split the text into an array of strings, cutting wherever there's a new line.
        string[] numberStrings = textFile.text.Split('\n');

        // Prepare a float array of the same size.
        numbers = new float[numberStrings.Length];
        // put floats in array
        for (int i = 0; i < numberStrings.Length; i++)
            numbers[i] = float.Parse(numberStrings[i]);

        /*
        foreach(var number in numbers)
        {
            Debug.Log(number);
        }
        */

        for (int j = 0; j < 100; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                CreateCylinderBetweenPoints(new Vector3(numbers[i + j] * (2 - (j / 100f)), numbers[i + j + 1] * (2 - (j / 100f)), numbers[i + j + 2] * (2 - (j / 100f))),
                                            new Vector3(numbers[i + j + 1] * (2 - (j / 100f)), numbers[i + j + 2] * (2 - (j / 100f)), numbers[i + j + 3] * (2 - (j / 100f))),
                                            100.0f, i, j);
                transform.localEulerAngles = new Vector3(0, (numbers[j]) / 1.5f, 0);
            }
        }

        gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        gameObject.transform.eulerAngles = new Vector3(0, 270, 0);
    }

    void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width, int i, int j)
    {
        GameObject group = GameObject.Find("ObjectGen_7");

        Vector3 offset = (end - start) / 2.0f;

        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Position it
        cylinder.transform.position = offset + start;
        // Scale it
        //Constant Width, Exponential Length
        //cylinder.transform.localScale = new Vector3(width, offset.magnitude * (j / 100f * (j / 100f) * 80f) + 1, width);
        cylinder.transform.localScale = new Vector3(((j / 100f * (j / 100f) + 0.01f) * width), offset.magnitude * (1 - (j / 100f) * (j / 100f)), ((j / 100f * (j / 100f) + 0.01f) * width));

        // Rotate it
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
        // Color it
        cylinder.GetComponent<Renderer>().material.color = new Color(1.0f - (j / 100f), 0, 1.0f - (i / 10f), 1);
        // Make it a child the object group
        cylinder.transform.parent = group.transform;
    }
}



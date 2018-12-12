using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DifferentGen : MonoBehaviour
{

    public TextAsset textFile;
    public float[] primes;

    public GameObject[] spheres;
    public GameObject[] cylinders;
 
    public int collection_1;
    public int collection_2;
    public int collection_3;
    public int collection_4;
    public int collection_5;
    public int collection_6;

    void Start()
    {

        // Split the text into an array of strings, cutting wherever there's a new line.
        string[] numberStrings = textFile.text.Split('\n');

        // Prepare a float array of the same size.
        primes = new float[numberStrings.Length];
        // put floats in array
        for (int i = 0; i < numberStrings.Length; i++)
            primes[i] = float.Parse(numberStrings[i]);

        spheres = new GameObject[20];
        cylinders = new GameObject[10];

        collection_1 = 2;
        collection_2 = 3;
        collection_3 = 5;
        collection_4 = 7;
        collection_5 = 11;
        collection_5 = 13;

        for (int i = 0; i < 20; i++)
        {
            if (i < 10)
            {
                GameObject newSphere = CreateSphere(new Vector3((primes[i + collection_1] - primes[collection_1]) / (primes[9 + collection_1] - primes[collection_1]) * 100.0f, (primes[i + collection_6] - primes[collection_6]) / (primes[9 + collection_6] - primes[collection_6]) * 100.0f, (primes[i + collection_3] - primes[collection_3]) / (primes[9 + collection_3] - primes[collection_3]) * 100.0f), 0.5f);
                spheres[i] = newSphere;

                GameObject newCylinder = CreateCylinderBetweenPoints(new Vector3((primes[i + collection_1] - primes[collection_1]) / (primes[9 + collection_1] - primes[collection_1]) * 100.0f, (primes[i + collection_6] - primes[collection_6]) / (primes[9 + collection_6] - primes[collection_6]) * 100.0f, (primes[i + collection_3] - primes[collection_3]) / (primes[9 + collection_3] - primes[collection_3]) * 100.0f), 
                                                                     new Vector3((primes[i + collection_2] - primes[collection_2]) / (primes[9 + collection_2] - primes[collection_2]) * 100.0f, (primes[i + collection_5] - primes[collection_5]) / (primes[9 + collection_5] - primes[collection_5]) * 100.0f, (primes[i + collection_4] - primes[collection_4]) / (primes[9 + collection_4] - primes[collection_4]) * 100.0f), 
                                                                     1.0f);
                cylinders[i] = newCylinder;
            }   
            else
            {
                GameObject newSphere = CreateSphere(new Vector3((primes[i-10 + collection_2] - primes[collection_2]) / (primes[9 + collection_2] - primes[collection_2]) * 100.0f, (primes[i-10 + collection_5] - primes[collection_5]) / (primes[9 + collection_5] - primes[collection_5]) * 100.0f, (primes[i-10 + collection_4] - primes[collection_4]) / (primes[9 + collection_4] - primes[collection_4]) * 100.0f), 0.5f);
                spheres[i] = newSphere;
            }

        }




        //  gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //  gameObject.transform.eulerAngles = new Vector3(0, 270, 0);
    }

    void Update()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i < 10)
            {
               // spheres[i].transform.position = new Vector3((primes[i + collection_1] - primes[collection_1]) / (primes[9 + collection_1] - primes[collection_1]) * 100.0f, (primes[i + collection_6] - primes[collection_6]) / (primes[9 + collection_6] - primes[collection_6]) * 100.0f, (primes[i + collection_3] - primes[collection_3]) / (primes[9 + collection_3] - primes[collection_3]) * 100.0f);
                RepositionSphere(spheres[i], i);

                RepositionCylinder(cylinders[i], i);

                //Vector3 start = new Vector3((primes[i + collection_1] - primes[collection_1]) / (primes[9 + collection_1] - primes[collection_1]) * 100.0f, (primes[i + collection_6] - primes[collection_6]) / (primes[9 + collection_6] - primes[collection_6]) * 100.0f, (primes[i + collection_3] - primes[collection_3]) / (primes[9 + collection_3] - primes[collection_3]) * 100.0f);
                //Vector3 end = new Vector3((primes[i + collection_2] - primes[collection_2]) / (primes[9 + collection_2] - primes[collection_2]) * 100.0f, (primes[i + collection_5] - primes[collection_5]) / (primes[9 + collection_5] - primes[collection_5]) * 100.0f, (primes[i + collection_4] - primes[collection_4]) / (primes[9 + collection_4] - primes[collection_4]) * 100.0f);
                //Vector3 offset = (end - start) / 2.0f;
                //cylinders[i].transform.position = offset + start;
                //cylinders[i].transform.localScale = new Vector3(1,offset.magnitude * 2, 1);
                //cylinders[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
            }
            else 
            {
                //spheres[i].transform.position = new Vector3((primes[i-10 + collection_2] - primes[collection_2]) / (primes[9 + collection_2] - primes[collection_2]) * 100.0f, (primes[i-10 + collection_5] - primes[collection_5]) / (primes[9 + collection_5] - primes[collection_5]) * 100.0f, (primes[i-10 + collection_4] - primes[collection_4]) / (primes[9 + collection_4] - primes[collection_4]) * 100.0f);
                RepositionSphere(spheres[i], i);
            }
        }

        if (Input.GetKeyDown("1"))
        {
            collection_1++;
        }
        if (Input.GetKeyDown("2"))
        {
            collection_2++;
        }
        if (Input.GetKeyDown("3"))
        {
            collection_3++;
        }
        if (Input.GetKeyDown("4"))
        {
            collection_4++;
        }
        if (Input.GetKeyDown("5"))
        {
            collection_5++;
        }
        if (Input.GetKeyDown("6"))
        {
            collection_6++;
        }

    }

    GameObject CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
    {
        GameObject group = GameObject.Find("ForestGen");

        Vector3 offset = (end - start) / 2.0f;

        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Position it
        cylinder.transform.position = offset + start;

        // Scale it
        cylinder.transform.localScale = new Vector3(width, offset.magnitude * 2, width);

        // Rotate it
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);

        // Color it
        //cylinder.GetComponent<Renderer>().material.color = new Color(1.0f - (j / 100f), 0, 1.0f - (i / 10f), 1);

        // Make it a child the object group
        cylinder.transform.parent = group.transform;

        return cylinder;
    }

    void RepositionCylinder(GameObject cylinder, int i)
    {
        Vector3 start = new Vector3((primes[i + collection_1] - primes[collection_1]) / (primes[9 + collection_1] - primes[collection_1]) * 100.0f, (primes[i + collection_6] - primes[collection_6]) / (primes[9 + collection_6] - primes[collection_6]) * 100.0f, (primes[i + collection_3] - primes[collection_3]) / (primes[9 + collection_3] - primes[collection_3]) * 100.0f);
        Vector3 end = new Vector3((primes[i + collection_2] - primes[collection_2]) / (primes[9 + collection_2] - primes[collection_2]) * 100.0f, (primes[i + collection_5] - primes[collection_5]) / (primes[9 + collection_5] - primes[collection_5]) * 100.0f, (primes[i + collection_4] - primes[collection_4]) / (primes[9 + collection_4] - primes[collection_4]) * 100.0f);
        Vector3 offset = (end - start) / 2.0f;
        cylinder.transform.position = offset + start;
        cylinder.transform.localScale = new Vector3(1, offset.magnitude * 10, 1);
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
    }

    GameObject CreateSphere(Vector3 position, float diameter)
    {
        GameObject group = GameObject.Find("ForestGen");

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Position it
        sphere.transform.position = position;

        // Scale it
        sphere.transform.localScale = new Vector3(diameter, diameter, diameter);

        // Color it
        // sphere.GetComponent<Renderer>().material.color = new Color(1.0f - (j / 100f), 0, 1.0f - (i / 10f), 1);

        // Make it a child the object group
        sphere.transform.parent = group.transform;

        return sphere;
    }

    void RepositionSphere(GameObject sphere, int i)
    {
        sphere.transform.position = i < 10
            ? new Vector3((primes[i + collection_1] - primes[collection_1]) / (primes[9 + collection_1] - primes[collection_1]) * 100.0f, (primes[i + collection_6] - primes[collection_6]) / (primes[9 + collection_6] - primes[collection_6]) * 100.0f, (primes[i + collection_3] - primes[collection_3]) / (primes[9 + collection_3] - primes[collection_3]) * 100.0f)
            : new Vector3((primes[i - 10 + collection_2] - primes[collection_2]) / (primes[9 + collection_2] - primes[collection_2]) * 100.0f, (primes[i - 10 + collection_5] - primes[collection_5]) / (primes[9 + collection_5] - primes[collection_5]) * 100.0f, (primes[i - 10 + collection_4] - primes[collection_4]) / (primes[9 + collection_4] - primes[collection_4]) * 100.0f);
    }
}



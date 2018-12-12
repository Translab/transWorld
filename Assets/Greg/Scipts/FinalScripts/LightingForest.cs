using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingForest : MonoBehaviour {

    public int numObjs;
    public float[] boundX = new float[2];
    public float[] boundY = new float[2];
    public float[] boundZ = new float[2];

    public float outOfBoundsRadius;
    //set a radius around (0,0,0) so kelp doesnt spawn in the room

    public GameObject myLight;
    public GameObject[] lightHolder;

    // Use this for initialization
    void Start()
    {
        lightHolder = new GameObject[100]; //spawn 

        placeTrees(boundX, boundY, boundZ, outOfBoundsRadius);

    }

    //place kelp pieces in space
    //  x,y,z are float[2] bounds and ob = out of bounds
    void placeTrees(float[] x, float[] y, float[] z, float ob)
    {
        for (int i = 0; i < lightHolder.Length; i++)
        {
            //sample and return a new position
            Vector3 newPosition = RandomPosition(x, y, z, ob);

            //Quaternion rotation = new Quaternion(0, 0, 0, 0);
            //rotation = rotation.Euler(0, 0, Random.Range(0f, 360f));

            lightHolder[i] = (GameObject)Instantiate(myLight, newPosition, Quaternion.identity, gameObject.transform);
        }
    }

    //compute a random position within passed in bounds, 
    //with outOfBounds being a radius where kelp won't grow
    Vector3 RandomPosition(float[] x, float[] y, float[] z, float outOfBounds)
    {
        //calculate a test location
        float newX = Random.Range(x[0], x[1]);
        float newY = Random.Range(y[0], y[1]);
        float newZ = Random.Range(z[0], z[1]);

        Vector3 testPosition = new Vector3(newX, newY, newZ);

        //return once the testPosition is out of the no spawn zone
        while (testPosition.magnitude < outOfBounds)
        {
            testPosition = RandomPosition(x, y, z, outOfBounds);
        }

        return testPosition;
    }
}

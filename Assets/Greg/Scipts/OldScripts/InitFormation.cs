using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitFormation : MonoBehaviour {

    public int ringIncrement;
    public int elementIncrement;
    public int radius;
    public GameObject ring;
    GameObject[] ringHolder;

    // Use this for initialization
    void Start() {
        ringHolder = GenerateRings(ringIncrement, elementIncrement, 360, 180, Vector3.up, 1);
    }


    //generate rings
    GameObject[] GenerateRings(int ringIncrement, int elementIncrement, int maxAngleEH, int maxAngleRH, Vector3 rotAxis, int radius)
    {
        //call posHelper then set the erings
        //Vector3[] positions = posHelper.GeneratePositions(ringIncrement, maxAngleRH, rotAxis);

        int numRings = maxAngleRH / ringIncrement; //total number of rings to instantiate
        //Vector3[] positions = new Vector3[numElements];
        int angle = 0;
        ringHolder = new GameObject[numRings];
        for (int i = 0; i < numRings; i++)
        {
            ringHolder[i] = (GameObject)Instantiate(ring, gameObject.transform);
            ringHolder[i].transform.Rotate(Vector3.right * angle);
            ringHolder[i].GetComponent<GenerateRing>().GenRing(elementIncrement, maxAngleEH, rotAxis, radius);
            angle = angle + ringIncrement;
            //gameObject.transform.Rotate(rotAxis * ringIncrement); //rotate 
            //ringHolder[i].GetComponent<Transform>().position = positions[i] * radius;
        }
        return ringHolder;
    }

}

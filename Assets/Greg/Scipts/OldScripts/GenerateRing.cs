using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRing : MonoBehaviour {

    public GameObject element;
    GameObject[] elementHolder;
    GenerateIncrementPositions posHelper;

    private void Awake()
    {
        posHelper = gameObject.GetComponent<GenerateIncrementPositions>();
    }

    public void GenRing(int elementIncrement, int maxAngle, Vector3 rotAxis, int radius)
    {
        Vector3[] positions = posHelper.GenerateElementPositions(elementIncrement, maxAngle, rotAxis);

        elementHolder = new GameObject[positions.Length];
        for (int i= 0; i< positions.Length; i++)
        {
            //Debug.Log(positions[i]);
            elementHolder[i] = (GameObject)Instantiate(element, gameObject.transform);
            elementHolder[i].GetComponent<Transform>().position = positions[i] * radius;
        }
        //elementHolder;
    }

    public GameObject[] GetElementHolder()
    {
        return elementHolder;
    }
}

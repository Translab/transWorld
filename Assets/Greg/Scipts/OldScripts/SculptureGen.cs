﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureGen : MonoBehaviour {

    private string start = "FX";
    private string sentence;
    public GameObject sculptureSystem;
    public GameObject[] sculpture;
    private Vector3[] initPositions;
    private Vector3[] endPositions;
    public float angle;
    public float length;
    public Vector3 tropism;
    public int numIter;
    public float randomness;
    private int numObjects = 0;

    private float[] boundX;
    private float[] boundY;
    private float[] boundZ;

    private Component[] rends;

    private Dictionary<char, string> rules = new Dictionary<char, string>();
    //private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();

    // Use this for initialization
    void Awake()
    {
        rules.Add('X', "X+YF+");
        rules.Add('Y', "-FX-Y");
        GenerateSystem(start);
        EvaluateSystem(randomness);

    }

    void Start()
    {
        //rends = gameObject.GetComponentsInChildren<Renderer>();
        //Debug.Log(rends);
        //foreach (Renderer dummyRend in rends)
        //{
            //Debug.Log("child rends");
        //    dummyRend.material.SetFloat("Amp", 0f);


        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EvaluateSystem(float rand)
    {
        // this function should
        // look at the distribution of the positions of the object
        //  

        for (int i = 0; i < sculpture.Length; i++)
        {

            float xNP = Random.Range(boundX[0], boundX[1]);
            float yNP = Random.Range(boundY[0], boundY[1]);
            float zNP = Random.Range(boundZ[0], boundZ[1]);
            //generate random point
            endPositions[i] = new Vector3(xNP, yNP, zNP);
            Vector3 currPos = initPositions[i];
            Vector3 newPoint = endPositions[i];
            //Vector3 currPos = sculpture[i].transform.position;
            //Vector3 newPoint = new Vector3(xNP, yNP, zNP);
            sculpture[i].transform.position = new Vector3(Mathf.Lerp(currPos.x, newPoint.x, rand), Mathf.Lerp(currPos.y, newPoint.y, rand), Mathf.Lerp(currPos.z, newPoint.z, rand));
            

            //Transform[] childRends = sculpture[i].GetComponentsInChildren<Transform>();
            //Debug.Log("child rends");
            sculpture[i].transform.parent = transform;

            //Transform[] childrens = sculpture[i].gameObject.GetComponentsInChildren<Transform>();
            //for (int q=0; q< childrens.Length; q++)
            //{
                //Debug.Log("inside childRends");
                //Renderer newRend = rend.GetComponent<Renderer>();
            //    childRends[q].materials[0].SetFloat(0, 0f);
                //newRend.material.SetFloat("_Frequencyx", Random.Range(-0.25f, 0.25f));
                //newRend.material.SetFloat("_Frequencyy", Random.Range(-0.25f, 0.25f));
                //newRend.material.SetFloat("_Frequencyz", Random.Range(-0.25f, 0.25f));
            //}


            //foreach (MeshRenderer rend in childRends)
            //{
            //    Debug.Log("inside childRends");
            //Renderer newRend = rend.GetComponent<Renderer>();
            //    rend.material.SetFloat("_MovementAmplitude", 0f);
            //newRend.material.SetFloat("_Frequencyx", Random.Range(-0.25f, 0.25f));
            //newRend.material.SetFloat("_Frequencyy", Random.Range(-0.25f, 0.25f));
            //newRend.material.SetFloat("_Frequencyz", Random.Range(-0.25f, 0.25f));
            //}
        }
    }

    void UpdateSystem(float rand)
    {


        // this function should
        // look at the distribution of the positions of the object
        //  

        for (int i = 0; i < sculpture.Length; i++)
        {
            //generate random point
            Vector3 currPos = initPositions[i];
            //Vector3 currPos = sculpture[i].transform.position;
            Vector3 newPoint = endPositions[i];

            sculpture[i].transform.position = new Vector3(Mathf.Lerp(currPos.x, newPoint.x, rand), Mathf.Lerp(currPos.y, newPoint.y, rand), Mathf.Lerp(currPos.z, newPoint.z, rand));
            //sculpture[i].
            sculpture[i].transform.parent = transform;

        }
    }

    float[] getBounds(float[] currBounds, float testVal)
    {
        if (testVal < currBounds[0])
        {
            currBounds[0] = testVal;
        }
        if (testVal > currBounds[1])
        {
            currBounds[1] = testVal;
        }

        return currBounds;
    }

    void GenerateSystem(string init)
    {
        sentence = GenerateSequence(init, numIter);
        CountDraws(sentence);
        DrawPattern(sentence);
        initPositions = new Vector3[sculpture.Length];
        endPositions = new Vector3[sculpture.Length];


        for (int i = 0; i < sculpture.Length; i++)
        {
            sculpture[i].transform.parent = transform;
            float xT = sculpture[i].transform.position.x;
            float yT = sculpture[i].transform.position.y;
            float zT = sculpture[i].transform.position.z;
            initPositions[i] = new Vector3(xT, yT, zT);

            if (i == 0)
            {
                //float xT = sculpture[i].transform.position.x;
                boundX = new float[2];
                boundX[0] = xT;
                boundX[1] = xT;

                //float yT = sculpture[i].transform.position.y;
                boundY = new float[2];
                boundY[0] = yT;
                boundY[1] = yT;

                //float zT = sculpture[i].transform.position.z;
                boundZ = new float[2];
                boundZ[0] = zT;
                boundZ[1] = zT;
            }
            else
            {
                boundX = getBounds(boundX, xT);
                boundY = getBounds(boundY, yT);
                boundZ = getBounds(boundZ, zT);
            }
            //Debug.Log(boundX);
            //Debug.Log(boundY);
            //Debug.Log(boundZ);

            //grab min/max
        }
    }

    string GenerateSequence(string init, int iter)
    {
        string outputSentence = init;
        for (int j = 0; j < iter; j++)
        {
            string newSentence = "";
            char[] stringCharacters = outputSentence.ToCharArray();

            for (int i = 0; i < stringCharacters.Length; i++)
            {
                char currentCharacter = stringCharacters[i];

                if (rules.ContainsKey(currentCharacter))
                {
                    newSentence += rules[currentCharacter];
                }
                else
                {
                    newSentence += currentCharacter.ToString();
                }

            }
            outputSentence = newSentence;
            //Debug.Log(outputSentence);
        }
        return outputSentence;
    }


    void CountDraws(string currSentence)
    {
        char[] stringCharacters = currSentence.ToCharArray();

        for (int i = 0; i < stringCharacters.Length; i++)
        {
            if (stringCharacters[i] == 'F')
            {
                numObjects = numObjects + 1;
            }

        }
    }

    void DrawPattern(string sentenceToDraw)
    {
        char[] stringCharacters = sentenceToDraw.ToCharArray();

        sculpture = new GameObject[numObjects];
        rends = new Renderer[numObjects];

        int n = 0;
        for (int i = 0; i < stringCharacters.Length; i++)
        {

            char currentCharacter = stringCharacters[i];
            if (currentCharacter == 'F')
            {
                Vector3 initialPosition = transform.position;
                transform.Translate(Vector3.forward * length);
                transform.Rotate(tropism);
                //Debug.Log(n);
                sculpture[n] = (GameObject)Instantiate(sculptureSystem, transform.position, transform.rotation);

                n = n + 1;
                //add in tropism vector T after drawing each segment

                //Debug.DrawLine(initialPosition, transform.position, Color.blue, 10000f, false);
            }
            //rotate
            else if (currentCharacter == '+')
            {
                transform.Rotate(Vector3.up * angle);
                //do something else
            }
            else if (currentCharacter == '-')
            {
                transform.Rotate(Vector3.up * -angle);
                //do something else
            }
        }
    }
}

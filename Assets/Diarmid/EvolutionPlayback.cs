using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class EvolutionPlayback : MonoBehaviour
{
    // public TextAsset textFile;
    public string parentObject = "GeneticGen";
    public string path = "geneRecord.txt";
    public List<float[]> animationData;
    public float[] frameData;
    int frame = 1;

    public float angleX;
    public float angleY;
    public float angleZ;

    //  private float nextActionTime = 0.0f;
    public float speed = 1;

    public float startTime;

    public Vector3 start;
    public Vector3 end;
    public Vector3 pStart;
    public Vector3 pEnd;
    public float width;
    public float pWidth;
    bool countUp = true;


    public GameObject[] cylinders;

    // Use this for initialization
    void Start()
    {
       // speed = Random.Range(.5f, 2f);

        animationData = new List<float[]>();

        // string[] numberStrings = textFile.text.Split('\n');


        using (StreamReader reader = new StreamReader(path))
        {

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] values = line.Split(',');

                frameData = new float[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    frameData[i] = float.Parse(values[i]);
                }
                int duplicate = 0;
                for (int i = 0; i < animationData.Count; i++)
                {
                    if (frameData == animationData[i])
                        duplicate++;
                }
                if (duplicate == 0)
                    animationData.Add(frameData);

            }
        }



        print("Frames:" + animationData.Count);

        cylinders = new GameObject[24];

        for (int i = 0; i < cylinders.Length; i++)
        {
            start = new Vector3(animationData[0][i * 4] * 30, animationData[0][1 + (i * 4)] * 90, animationData[0][2 + (i * 4)] * 30);
            end = new Vector3(animationData[0][(i * 4) + 4] * 30, animationData[0][(i * 4) + 5] * 90, animationData[0][(i * 4) + 6] * 30);
            width = (animationData[0][(i * 4) + 3] + animationData[0][(i * 4) + 7]);

            cylinders[i] = CreateCylinderBetweenPoints(start, end, width);
        }

        transform.RotateAround(Vector3.zero, Vector3.left, angleX);
        transform.RotateAround(Vector3.zero, Vector3.up, angleY);
        transform.RotateAround(Vector3.zero, Vector3.forward, angleZ);

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > startTime + (1/speed))
        {
            //nextActionTime += period;
            // execute block of code here

            startTime = Time.time;
            print("frame:" + frame);
            if (countUp == true)
                frame++;
            else 
                frame--;
            if (frame == animationData.Count - 1)
                countUp = false;
            if (frame == 1)
                countUp = true;
         //   frame = frame % (animationData.Count - 1);
        }

        for (int i = 0; i < cylinders.Length; i++)
        {
            if (countUp == true)
            {
                pStart = new Vector3(animationData[frame][i * 4] * 30, animationData[frame][1 + (i * 4)] * 90, animationData[frame][2 + (i * 4)] * 30);
                pEnd = new Vector3(animationData[frame][(i * 4) + 4] * 30, animationData[frame][(i * 4) + 5] * 90, animationData[frame][(i * 4) + 6] * 30);
                pWidth = (animationData[frame][(i * 4) + 3] + animationData[frame][(i * 4) + 7]);

                start = new Vector3(animationData[frame + 1][i * 4] * 30, animationData[frame + 1][1 + (i * 4)] * 90, animationData[frame + 1][2 + (i * 4)] * 30);
                end = new Vector3(animationData[frame + 1][(i * 4) + 4] * 30, animationData[frame + 1][(i * 4) + 5] * 90, animationData[frame + 1][(i * 4) + 6] * 30);
                width = (animationData[frame + 1][(i * 4) + 3] + animationData[frame + 1][(i * 4) + 7]);
            }
           else
            {
                pStart = new Vector3(animationData[frame][i * 4] * 30, animationData[frame][1 + (i * 4)] * 90, animationData[frame][2 + (i * 4)] * 30);
                pEnd = new Vector3(animationData[frame][(i * 4) + 4] * 30, animationData[frame][(i * 4) + 5] * 90, animationData[frame][(i * 4) + 6] * 30);
                pWidth = (animationData[frame][(i * 4) + 3] + animationData[frame][(i * 4) + 7]);

                start = new Vector3(animationData[frame - 1][i * 4] * 30, animationData[frame -1][1 + (i * 4)] * 90, animationData[frame-1][2 + (i * 4)] * 30);
                end = new Vector3(animationData[frame - 1][(i * 4) + 4] * 30, animationData[frame -1][(i * 4) + 5] * 90, animationData[frame-1][(i * 4) + 6] * 30);
                width = (animationData[frame- 1][(i * 4) + 3] + animationData[frame -1][(i * 4) + 7]);

            }
            LerpCylinder(cylinders[i], start, end, width, pStart, pEnd, pWidth, startTime);

        }


        transform.RotateAround(Vector3.zero, Vector3.left, angleX);
        transform.RotateAround(Vector3.zero, Vector3.up, angleY);
        transform.RotateAround(Vector3.zero, Vector3.forward, angleZ);

    }

    GameObject CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
    {
        GameObject group = GameObject.Find(parentObject);

        Vector3 offset = (end - start) / 2.0f;
      
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
     
        // Position it
        cylinder.transform.position = offset + start;
       
        // Scale it
        cylinder.transform.localScale = new Vector3(width, offset.magnitude, width);
       
        // Rotate it
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);

        cylinder.transform.parent = group.transform;

        return cylinder;
    }

    void RepositionCylinder(GameObject cylinder, Vector3 start, Vector3 end, float width)
    {
        Vector3 offset = (end - start) / 2.0f;
        cylinder.transform.position = offset + start;
        cylinder.transform.localScale = new Vector3(width, offset.magnitude, width);
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
    }

    void LerpCylinder(GameObject cylinder, Vector3 start, Vector3 end, float width, Vector3 pStart, Vector3 pEnd, float pWidth, float startTime)
    {
        float fracJourney = (Time.time - startTime) * speed;
        Vector3 offset = (end - start) / 2.0f;
        Vector3 pOffset = (pEnd - pStart) / 2.0f;
        Vector3 position = offset + start;
        Vector3 pPosition = pOffset + pStart;
        Vector3 scale = new Vector3(width, offset.magnitude, width);
        Vector3 pScale = new Vector3(pWidth, pOffset.magnitude, pWidth);
        Vector3 toDirection = end - start;
        Vector3 pToDirection = pEnd - pStart;
        Vector3 rotation = Vector3.Lerp(pToDirection, toDirection, fracJourney);

        cylinder.transform.position = Vector3.Lerp(pPosition, position, fracJourney);
        cylinder.transform.localScale = Vector3.Lerp(pScale, scale, fracJourney);
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, rotation);

        
    }

}




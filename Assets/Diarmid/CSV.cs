using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSV : MonoBehaviour
{

    //private const string FILE_NAME = "MyFile.txt";

    //  List<string> newArray;
    //List<float> newArray = new List<float>();
    // Use this for initialization
    void Start()
    {
     //   float[] row1 = { 1, 2, 3, 4, 5 };
     //   float[] row2 = { 2, 3, 4, 5, 6 };
      //  float[] row3 = { 3, 4, 5, 6, 7 };


      //  List<float[]> geneRecord = new List<float[]>();
      //  genes.Add(row1);
      //  genes.Add(row2);
      //  genes.Add(row3);

      //  ArrayToCSV2(genes, "newFile.txt");
        /*
        float[] scores = { 97f, 92f, 8f, 60f };
        string fileName = "newFile.txt";
        ArrayToCSV(scores, fileName);
        /*
        StreamWriter sr = File.CreateText(FILE_NAME);
        for (int i = 0; i < scores.Length; i++)
        {
            sr.WriteLine("This is an array {0}",
                scores[i]);
        }
        sr.Close();
        

        using (StreamReader reader = new StreamReader(fileName))
        {

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                foreach (var value in values)
                    newArray.Add(float.Parse(value));

            }
        }
        foreach (float f in newArray)
            print(f);
        */
    }


    // Update is called once per frame
    void Update()
    {

    }

    void ArrayToCSV(float[] arr, string fileName)
    {
        using (StreamWriter file = new StreamWriter(fileName))
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != arr.Length - 1)
                    file.Write(arr[i] + ",");
                else if (i == arr.Length - 1)
                    file.Write(arr[i]);
            }
        }
    }

    void ArrayToCSV2(List<float[]> arr, string fileName)
    {
        using (StreamWriter file = new StreamWriter(fileName))
        {
            for (int i = 0; i < arr.Count; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                    if (j < arr[i].Length - 1)
                        file.Write(arr[i][j] + ",");
                    else 
                        file.Write(arr[i][j] + System.Environment.NewLine);
            }
        }
    }




}


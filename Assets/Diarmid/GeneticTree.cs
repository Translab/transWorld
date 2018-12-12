using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneticTree : MonoBehaviour
{

    public float mutationRate = 0.02f;
    public int totalPopulation = 50;
    public GameObject[] cylinders;

    public DNA[] population;
    public DNA elite;
    public Color[] colors;

    public string path;
    public List<float[]> geneRecord;

    public float position;
    int generation;
    public float bestScore;
    public float previousBest;
    int offset = 0;
    

    public class DNA
    {
        public float[] genes;
        public float fitness;

        // constructor: create DNA randomly
        public DNA(int num)
        {
            genes = new float[num];

            for (int i = 0; i < genes.Length; i++)
                genes[i] = Random.Range(0f, 0.01f);

        }

        // calculate fitness
        public void CalculateFitness()
        {
            float score = 0;
            float previousDiameter = genes[3];
            float previousY = genes[1];
            for (int i = 0; i < genes.Length; i += 4)
            {
                for (int j = 0; j < genes.Length; j += 4)
                {
                    if (j < i)
                    {
                        if (genes[j + 1] < genes[i + 1])
                            score++;
                        if (genes[j + 3] > genes[i + 3])
                            score++;
                    }
                    else if (j > i)
                    {
                        if (genes[j + 1] > genes[i + 1])
                            score++;
                        if (genes[j + 3] < genes[i + 3])
                            score++;
                    }
                    score += Vector3.Distance(new Vector3(genes[i], genes[i + 1], genes[i + 2]), new Vector3(genes[j], genes[j + 1], genes[j + 2]));
                }

            }

            fitness = (score / 1400f) * (score / 1400f);
        }

        // crossover
        public DNA Crossover(DNA partner)
        {
            DNA child = new DNA(genes.Length);
            int midpoint = Random.Range(0, genes.Length);

            for (int i = 0; i < genes.Length; i++)
            {
                if (i > midpoint)
                {
                    child.genes[i] = genes[i];
                }
                else child.genes[i] = partner.genes[i];
            }
            return child;
        }

        // mutation
        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < genes.Length; i++)
            {
                if (Random.Range(0f, 1f) < mutationRate)
                {
                    genes[i] = genes[i] + Random.Range(0f, 0.01f);
                }
            }
        }
    }


    void Start()
    {
        population = new DNA[totalPopulation];
        colors = new Color[totalPopulation];
        cylinders = new GameObject[24];
        elite = new DNA(100);
        geneRecord = new List<float[]>();

        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new DNA(100);
            colors[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0, 1f));
        }
        elite.genes[0] = 0f;
        elite.genes[1] = 0f;
        elite.genes[2] = 0f;
        elite.genes[3] = 0f;
        for (int i = 0; i < cylinders.Length; i++)
        {
            Vector3 start = new Vector3(elite.genes[i * 4] * 30 + offset, elite.genes[1 + (i * 4)] * 90, elite.genes[2 + (i * 4)] * 30);
            Vector3 end = new Vector3(elite.genes[(i * 4) + 4] * 30 + offset, elite.genes[(i * 4) + 5] * 90, elite.genes[(i * 4) + 6] * 30);
            float diameter = (elite.genes[(i * 4) + 3] + elite.genes[(i * 4) + 7]);

            cylinders[i] = CreateCylinderBetweenPoints(start, end, diameter);
        }
        print(cylinders.Length);
    }

    void Update()
    {
        // selection
        // calculate fitness
        elite.genes[0] = 0;
        elite.genes[1] = 0;
        elite.genes[2] = 0;

        population[0] = elite;
        for (int i = 0; i < population.Length; i++)
        {
            population[i].CalculateFitness();

            if (population[i].fitness > bestScore)
            {

                bestScore = population[i].fitness;
                elite = population[i];
                geneRecord.Add(elite.genes);
            }
            print("Best Fitness: " + bestScore);
        }

        // build mating pool
        List<DNA> matingPool = new List<DNA>();

        for (int i = 0; i < population.Length; i++)
        {
            int n = (int)(population[i].fitness * 1000f);

            for (int j = 0; j < n; j++)
            {
                matingPool.Add(population[i]);
            }
        }

        // reproduction
        for (int i = 0; i < population.Length; i++)
        {
            int a = Random.Range(0, matingPool.Count);
            int b = Random.Range(0, matingPool.Count);
            DNA partnerA = matingPool[a];
            DNA partnerB = matingPool[b];

            // crossover
            DNA child = partnerA.Crossover(partnerB);

            //mutation
            child.Mutate(mutationRate);

            population[i] = child;
        }


        // display


        print("Generation: " + generation);

        if (bestScore > previousBest)
        {
            for (int i = 0; i < cylinders.Length; i++)
            {
                Vector3 start = new Vector3(elite.genes[i*4] * 30 + offset, elite.genes[1 + (i*4)] * 90, elite.genes[2 + (i * 4)] * 30);
                Vector3 end = new Vector3(elite.genes[(i*4) + 4] * 30 + offset, elite.genes[(i * 4) + 5] * 90, elite.genes[(i * 4) + 6] * 30);
                float diameter = (elite.genes[(i * 4) + 3] + elite.genes[(i * 4) + 7]);

                RepositionCylinder(cylinders[i], start, end, diameter);
              //  CreateCylinderBetweenPoints(start, end, diameter);
              //  CreateSphere(start, diameter);
              //  CreateSphere(end, diameter);
            }
           // offset += 50;
        }

        previousBest = bestScore;
        print("Frames Captured:" + geneRecord.Count);
        if (geneRecord.Count == 200)
        {
            ArrayToCSV(geneRecord, path);
            print("############### FILE WRITTEN ###############");
        }

        generation++;

    }

    GameObject CreateSphere(Vector3 location, float diameter)
    {

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Position it
        sphere.transform.position = location;

        // Scale it
        sphere.transform.localScale = new Vector3(diameter, diameter, diameter);

        // Color it
        // sphere.GetComponent<Renderer>().material.color = new Color(1.0f - (j / 100f), 0, 1.0f - (i / 10f), 1);

        return sphere;
    }

    GameObject CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
    {
        GameObject group = GameObject.Find("GeneticGen");

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

    void ArrayToCSV(List<float[]> arr, string fileName)
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






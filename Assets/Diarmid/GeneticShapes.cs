using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticShapes : MonoBehaviour
{

    public float mutationRate = 0.02f;
    public int totalPopulation = 10;
    public GameObject[,] spheres;

    public DNA[] population;
    //public List<DNA> matingPool;
    public Vector3[] target;
    public Color[] colors;

    public float position;
    int generation;

    public class DNA
    {
        public Vector3[] genes;
        public float fitness;
        public Color color;

        // constructor: create DNA randomly
        public DNA(int num)
        {

            genes = new Vector3[num];

            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = new Vector3(Random.Range(0,10), Random.Range(0, 10), Random.Range(0, 10));
            }

            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        // calculate fitness
        public void CalculateFitness(Vector3[] target)
        {
            int score = 0;
            for (int i = 0; i < genes.Length; i++)
            {
                if (genes[i] == target[i])
                    score ++;
            }

            fitness = ((float)score / (float)target.Length) * ((float)score / (float)target.Length) * ((float)score / (float)target.Length) * ((float)score / (float)target.Length) + 0.0002f;

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
                    genes[i] = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
                }
            }
        }
    }


    void Start()
    {

        target = new Vector3[10];
        for (int i = 0; i < target.Length; i++)
        {
            target[i] = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            GameObject newSphere = CreateSphere(new Vector3(target[i].x + (15 * 5), target[i].y + 15, target[i].z), 1);
        }
        

        population = new DNA[totalPopulation];

        print(population.Length);
        spheres = new GameObject[population.Length, target.Length];
        colors = new Color[totalPopulation];


        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new DNA(target.Length);
            colors[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0, 1f));
        }

        for (int i = 0; i < population.Length; i++)
        {
            for (int j = 0; j < target.Length; j++)
            {
                GameObject newSphere = CreateSphere(population[i].genes[j], 1);
                spheres[i, j] = newSphere;
            }
        }
    }

    void Update()
    {
        // selection
        // calculate fitness
        for (int i = 0; i < population.Length; i++)
        {
            population[i].CalculateFitness(target);

        }

        // build mating pool
        List<DNA> matingPool = new List<DNA>();

        for (int i = 0; i < population.Length; i++)
        {
            int n = (int)(population[i].fitness * 10000f);
            print(n);
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

        for (int i = 0; i < population.Length; i++)
        {

            for (int j = 0; j < target.Length; j++)
            {
                Vector3 temp = new Vector3(population[i].genes[j].x + (15 * i), population[i].genes[j].y, population[i].genes[j].z);

                spheres[i, j].transform.position = temp;
                if (population[i].genes[j] == target[j])
                    spheres[i, j].GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                else
                    spheres[i, j].GetComponent<Renderer>().material.color = colors[i];
            }

        }


        print(generation);




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


}





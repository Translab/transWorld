using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticShapes2 : MonoBehaviour
{

    public float mutationRate = 0.02f;
    public int totalPopulation = 10;
    public GameObject[,] spheres;

    public DNA[] population;
    //public List<DNA> matingPool;
    public int[] target;
    public Color[] colors;

    public float position;
    int generation;

    public class DNA
    {
        public int[] genes;
        public float fitness;


        // constructor: create DNA randomly
        public DNA(int num)
        {

            genes = new int[num];

            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = Random.Range(0, 10);
            }
        }

        // calculate fitness
        public void CalculateFitness(int[] target)
        {
            int score = 0;
            for (int i = 0; i < genes.Length; i++)
            {
                if (genes[i] == target[i])
                    score++;
            }
            //fitness = ((float)score / (float)target.Length) * ((float)score / (float)target.Length) * ((float)score / (float)target.Length) * ((float)score / (float)target.Length);
            fitness = score;
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
                    genes[i] = Random.Range(0, 10);
                }
            }
        }
    }


    void Start()
    {

        target = new int[30];
        for (int i = 0; i < target.Length; i++)
        {
            target[i] = Random.Range(0, 10);
        }

        population = new DNA[totalPopulation];

        print(population.Length);
        spheres = new GameObject[population.Length, target.Length/3];
        colors = new Color[totalPopulation];


        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new DNA(target.Length);
            colors[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0, 1f));
        }

        for (int i = 0; i < population.Length; i++)
        {

            GameObject newSphere0 = CreateSphere(new Vector3(population[i].genes[0], population[i].genes[1], population[i].genes[2]), 1);
            spheres[i, 0] = newSphere0;
            GameObject newSphere1 = CreateSphere(new Vector3(population[i].genes[3], population[i].genes[4], population[i].genes[5]), 1);
            spheres[i, 1] = newSphere1;
            GameObject newSphere2 = CreateSphere(new Vector3(population[i].genes[6], population[i].genes[7], population[i].genes[8]), 1);
            spheres[i, 2] = newSphere2;
            GameObject newSphere3 = CreateSphere(new Vector3(population[i].genes[9], population[i].genes[10], population[i].genes[11]), 1);
            spheres[i, 3] = newSphere3;
            GameObject newSphere4 = CreateSphere(new Vector3(population[i].genes[12], population[i].genes[13], population[i].genes[14]), 1);
            spheres[i, 4] = newSphere4;
            GameObject newSphere5 = CreateSphere(new Vector3(population[i].genes[15], population[i].genes[16], population[i].genes[17]), 1);
            spheres[i, 5] = newSphere5;
            GameObject newSphere6 = CreateSphere(new Vector3(population[i].genes[18], population[i].genes[19], population[i].genes[20]), 1);
            spheres[i, 6] = newSphere6;
            GameObject newSphere7 = CreateSphere(new Vector3(population[i].genes[21], population[i].genes[22], population[i].genes[23]), 1);
            spheres[i, 7] = newSphere7;
            GameObject newSphere8 = CreateSphere(new Vector3(population[i].genes[24], population[i].genes[25], population[i].genes[26]), 1);
            spheres[i, 8] = newSphere8;
            GameObject newSphere9 = CreateSphere(new Vector3(population[i].genes[27], population[i].genes[28], population[i].genes[29]), 1);
            spheres[i, 9] = newSphere9;

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
            int n = (int)(population[i].fitness * 1f); // + 1;

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
            spheres[i, 0].transform.position = new Vector3(population[i].genes[0] + (11 * i), population[i].genes[1], population[i].genes[2]);
            spheres[i, 1].transform.position = new Vector3(population[i].genes[3] + (11 * i), population[i].genes[4], population[i].genes[5]);
            spheres[i, 2].transform.position = new Vector3(population[i].genes[6] + (11 * i), population[i].genes[7], population[i].genes[8]);
            spheres[i, 3].transform.position = new Vector3(population[i].genes[9] + (11 * i), population[i].genes[10], population[i].genes[11]);
            spheres[i, 4].transform.position = new Vector3(population[i].genes[12] + (11 * i), population[i].genes[13], population[i].genes[14]);
            spheres[i, 5].transform.position = new Vector3(population[i].genes[15] + (11 * i), population[i].genes[16], population[i].genes[17]);
            spheres[i, 6].transform.position = new Vector3(population[i].genes[18] + (11 * i), population[i].genes[19], population[i].genes[20]);
            spheres[i, 7].transform.position = new Vector3(population[i].genes[21] + (11 * i), population[i].genes[22], population[i].genes[23]);
            spheres[i, 8].transform.position = new Vector3(population[i].genes[24] + (11 * i), population[i].genes[25], population[i].genes[26]);
            spheres[i, 9].transform.position = new Vector3(population[i].genes[27] + (11 * i), population[i].genes[28], population[i].genes[29]);

        }

        for (int i = 0; i < population.Length; i++)
        {

            for (int j = 0; j < target.Length; j += 3)
            {
                int count = 0;
                if (population[i].genes[j] == target[j])
                    count++;
                if (population[i].genes[j+1] == target[j+1])
                    count++;
                if (population[i].genes[j+2] == target[j+2])
                    count++;
                if (count == 1)
                    spheres[i, j / 3].GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
                if (count == 2)
                    spheres[i, j / 3].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0f);
                if (count == 3)
                    spheres[i, j / 3].GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
                if (count == 0)
                    spheres[i, j / 3].GetComponent<Renderer>().material.color = new Color(0.25f, 0.25f, 0.25f);//colors[i];
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





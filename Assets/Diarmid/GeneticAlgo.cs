using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo : MonoBehaviour
{

    public float mutationRate = 0.02f;
    public int totalPopulation = 10;
    public GameObject[,] spheres;

    public DNA[] population;
    //public List<DNA> matingPool;
    public string target;

    public float position;
    int generation;

    public class DNA
    {
        public char[] genes;
        public float fitness;

        // constructor: create DNA randomly
        public DNA(int num)
        {

            genes = new char[num];

            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = (char)Random.Range(32, 122);
            }
        }

        // converts character array to string
        public string GetPhrase()
        {
            return new string(genes);
        }

        // calculate fitness
        public void CalculateFitness(string target)
        {
            int score = 0;
            for (int i = 0; i < genes.Length; i++)
            {
                if (genes[i] == target[i])
                    score++;
            }
            fitness = ((float)score / (float)target.Length) * ((float)score / (float)target.Length) * ((float)score / (float)target.Length) * ((float)score / (float)target.Length);
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
                    genes[i] = (char)Random.Range(32, 122);
                }
            }
        }



    }


    void Start()
    {
        target = "Ten Beads!";

        population = new DNA[totalPopulation];
        print(population.Length);
        spheres = new GameObject[population.Length,target.Length];
       

        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new DNA(target.Length);
        }

        for (int i = 0; i < population.Length; i++)
        {
            for (int j = 0; j < target.Length; j++)
            {
                position = population[i].genes[j];
                GameObject newSphere = CreateSphere(new Vector3(position, position, position), 2); 
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
            int n = (int)(population[i].fitness * 10000f) + 1;

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
                position = population[i].genes[j];
                spheres[i, j].transform.position = new Vector3(position, i * 10, 0);
                if (population[i].genes[j] == target[j])
                    spheres[i, j].GetComponent<Renderer>().material.color = new Color(1,0,0);
                else
                    spheres[i, j].GetComponent<Renderer>().material.color = new Color(1, 1, 1);
            }

        }


        print(generation);
           
        string everything = "";
        for (int i = 0; i < population.Length; i++)
        {
            everything += population[i].GetPhrase() + "   ";
            if (population[i].GetPhrase() == target)
                print("+++++++++TARGET REACHED+++++++++");
        }


        print(everything);


        generation++;
    }

    GameObject CreateSphere(Vector3 position, float diameter)
    {
       

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Position it
        sphere.transform.position = position;

        // Scale it
        sphere.transform.localScale = new Vector3(diameter, diameter, diameter);

        // Color it
        // sphere.GetComponent<Renderer>().material.color = new Color(1.0f - (j / 100f), 0, 1.0f - (i / 10f), 1);

        return sphere;
    }


}





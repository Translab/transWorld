using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticShapes3 : MonoBehaviour
{

    public float mutationRate = 0.02f;
    public int totalPopulation = 50;
    public GameObject[,] spheres;

    public DNA[] population;
    public DNA elite;
    //public List<DNA> matingPool;
    public int[,] target;
    public Color[] colors;

    public float position;
    int generation;
    public float bestScore;
    public float previousBest;
    int offset = 0;

    public class DNA
    {
        public int[,] genes;
        public float fitness;

        // constructor: create DNA randomly
        public DNA(int num)
        {

            genes = new int[num, 3];

            for (int i = 0; i < genes.GetLength(0); i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    genes[i, j] = Random.Range(0, 50);
                }
            }
        }

        // calculate fitness
        public void CalculateFitness()
        {
            float score = 0;
            for (int i = 0; i < genes.GetLength(0); i++)
            {

                float subScore = 0;
                for (int j = 0; j < genes.GetLength(0); j++)
                {
                    Vector3 start = new Vector3(genes[i, 0], genes[i, 1], genes[i, 2]);
                    Vector3 end = new Vector3(genes[j, 0], genes[j, 1], genes[j, 2]);


                    if (Mathf.Approximately(Vector3.Distance(start, end), 2))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 3))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 5))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 7))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 11))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 13))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 17))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 19))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 23))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 29))
                        subScore++;
                    if (Mathf.Approximately(Vector3.Distance(start, end), 31))
                        subScore++;

                    if (j != i) {
                        if (start == end)
                            score = 0;
                    }


                }
                score += subScore;



            }
            fitness = (score / 100f)*(score / 100f); 
            /*
            float score = 0;
            for (int i = 0; i < genes.GetLength(0); i++)
            {

                float subScore = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (genes[i, j] == target[i, j])
                        subScore++;
                }
                subScore = (subScore * subScore) / 90;
                score += subScore;
a
             }
            fitness = score * score * score * score;
            //fitness = (float)System.Math.Pow(2, score);  
            //fitness = ((float)score / (float)92.61) * ((float)score / (float)92.61) * ((float)score / (float)92.61) * ((float)score / (float)92.61);// + 0.0002f;
          */
        }

        // crossover
        public DNA Crossover(DNA partner)
        {
            DNA child = new DNA(genes.GetLength(0));

            int midpoint = Random.Range(0, 3);

            for (int i = 0; i < genes.GetLength(0); i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j > midpoint)
                    {
                        child.genes[i, j] = genes[i, j];
                    }
                    else child.genes[i, j] = partner.genes[i, j];
                }
            }
            return child;
        }

        // mutation
        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < genes.GetLength(0); i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Random.Range(0f, 1f) < mutationRate)
                    {
                        genes[i, j] = Random.Range(0, 50);
                    }
                }
            }
        }
    }


    void Start()
    {
        /*
        target = new int[10, 3];
        for (int i = 0; i < target.GetLength(0); i++)
        {
            for (int j = 0; j < 3; j++)
            {
                target[i, j] = Random.Range(0, 30);
            }
          //  GameObject newSphere = CreateSphere(new Vector3(target[i, 0] + (30 * 5), target[i, 1] + 30, target[i, 2]), 1);
        }
*/
        population = new DNA[totalPopulation];
       // print(target.GetLength(0));
        print(population.Length);
       // spheres = new GameObject[population.Length, target.GetLength(0)];
        colors = new Color[totalPopulation];

        elite = new DNA(10);

        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new DNA(10);
            colors[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0, 1f));
        }
        /*
        for (int i = 0; i < population.Length; i++)
        {
            for (int j = 0; j < target.GetLength(0); j++)
            {
                GameObject newSphere = CreateSphere(new Vector3(population[i].genes[j, 0], population[i].genes[j, 1], population[i].genes[j, 2]), 1);
                spheres[i, j] = newSphere;
            }
        }
        */

    }

    void Update()
    {


        // selection
        // calculate fitness
        // float generationBest = 0;
        population[0] = elite;
        for (int i = 0; i < population.Length; i++)
        {
            population[i].CalculateFitness();

            if (population[i].fitness > bestScore)
            {
                bestScore = population[i].fitness;
                elite = population[i];
               

            }
            print("Best Fitness: " + bestScore);


            /*
            float count = 0;

            for (int j = 0; j < target.GetLength(0); j++)
            {
                if (population[i].genes[j, 0] == target[j, 0] && population[i].genes[j, 1] == target[j, 1] && population[i].genes[j, 2] == target[j, 2])
                    count++;
            }
            if (count > generationBest)
            {
                generationBest = count;
                elite = population[i];
            }
            print("Generation Best: " + generationBest);
            */
        }


        // build mating pool
        List<DNA> matingPool = new List<DNA>();

        for (int i = 0; i < population.Length; i++)
        {
            int n = (int)(population[i].fitness * 10000f);

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
        /*
        for (int i = 0; i < population.Length; i++)
        {

            for (int j = 0; j < target.GetLength(0); j++)
            {
                Vector3 temp = new Vector3(population[i].genes[j, 0] + (30 * i), population[i].genes[j, 1], population[i].genes[j, 2]);

                spheres[i, j].transform.position = temp;
                spheres[i, j].GetComponent<Renderer>().material.color = colors[i];
                /*
                int count = 0;
                for (int k = 0; k < 3; k++)
                {
                    if (population[i].genes[j, k] == target[j, k])
                    {
                        count++;
                        // spheres[i, j].GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                    }
                    else
                        spheres[i, j].GetComponent<Renderer>().material.color = colors[i];
                }
                if (count == 0)
                    spheres[i, j].GetComponent<Renderer>().material.color = colors[i];
                if (count == 1)
                    spheres[i, j].GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                if (count == 2)
                    spheres[i, j].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0);
                if (count == 3)
                    spheres[i, j].GetComponent<Renderer>().material.color = new Color(0, 1, 0);

            }

        }*/

        print("Generation: " + generation);

        if (bestScore > previousBest) {

            for (int i = 0; i < elite.genes.GetLength(0); i++)
            {
               // GameObject newSphere = CreateSphere(new Vector3(elite.genes[i, 0] + offset, elite.genes[i, 1] - 30, elite.genes[i, 2]), 1);

                for (int j = 0; j < elite.genes.GetLength(0); j++)
                {
                    Vector3 start = new Vector3(elite.genes[i, 0] + offset, elite.genes[i, 1], elite.genes[i, 2]);
                    Vector3 end = new Vector3(elite.genes[j, 0] + offset, elite.genes[j, 1], elite.genes[j, 2]);



                    if (Mathf.Approximately(Vector3.Distance(start, end), 2))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 3))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 5))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 7))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 11))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 13))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 17))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 19))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 23))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 29))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }
                    if (Mathf.Approximately(Vector3.Distance(start, end), 31))
                    {
                        CreateCylinderBetweenPoints(start, end, 0.5f);
                        CreateSphere(start, 1);
                        CreateSphere(end, 1);
                    }

                }
            }
            offset += 50;
        }



        previousBest = bestScore;
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
        Vector3 offset = (end - start) / 2.0f;

        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Position it
        cylinder.transform.position = offset + start;

        // Scale it
        cylinder.transform.localScale = new Vector3(width, offset.magnitude, width);

        // Rotate it
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);

        return cylinder;
    }


}





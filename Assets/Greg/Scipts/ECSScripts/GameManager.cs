using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Greg
{
    public class GameManager : MonoBehaviour {

        public static GameManager instance = null;
        EntityManager em;



        //init parameters
        public int maxIter;
        private int currIter;
        private int currDrawDepth;

        public int ringsPerSystem;
        public int spawnsPerRing;
        public int cubesPerRing;

        public GameObject cubePrefab;
        public GameObject spawnPrefab;
        public GameObject worldPrefab;

        private int spawnsPerSystem;
        private int cubesPerSystem;

        private int spawnCounter;
        private int cubeCounter;

        private NativeArray<Entity> spawnEntities; //holds all spawn points
        private NativeArray<Entity> cubeEntities; //holds all cube entities
        private NativeArray<Entity> worldEntity; //parent to everything, is root


        //public float stuff1 = 1;
        //public float stuff2 = 1;
        //public float moveSpeed = 1;


        void Awake () {
            //singleton stuffs
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            //grab the entity manager
            em = World.Active.GetOrCreateManager<EntityManager>();
            
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Debug.Log("Game Manager start");
            InitWorld();
        }

        void InitWorld()
        {
            spawnsPerSystem = ringsPerSystem * spawnsPerRing;
            cubesPerSystem = ringsPerSystem * cubesPerRing;

            int numSpawns = ComputeNumSpawns(spawnsPerSystem, maxIter); //get total number of spawn entities
            int numCubes = ComputeNumCubes(spawnsPerSystem, cubesPerSystem, maxIter); //get total number of cubes
            spawnEntities = new NativeArray<Entity>(numSpawns, Allocator.Persistent); //preallocate spawn entity arrays
            cubeEntities = new NativeArray<Entity>(numCubes, Allocator.Persistent); //preallocate cube entity arrays
            worldEntity = new NativeArray<Entity>(1, Allocator.Persistent); //world entity

            //set init variables
            currIter = 1;
            cubeCounter = 0;
            spawnCounter = 0;

            worldEntity[0] = em.Instantiate(worldPrefab); //instantiate world entity
            GenerateSystem(ringsPerSystem, 180, true, worldEntity[0]); //start the recursive process
           
        }

        //number of spawns
        int ComputeNumSpawns(int perSys, int numIter)
        {
            int returnNum = 0;
            for (int i=0; i<numIter; i++)
            {
                returnNum = returnNum + (int)Mathf.Pow(perSys, numIter);
            }
            return returnNum;
        }

        //number of cubes 
        int ComputeNumCubes(int spawnsPerSys, int cubesPerSys, int numIter)
        {
            return (int)Mathf.Pow(spawnsPerSys, numIter) * cubesPerSys;
        }



        public void GenerateCube(int cubeIdx, Vector3 currRotation, Entity parentEntity)
        {
            cubeEntities[cubeIdx] = em.Instantiate(cubePrefab); //instantiate entity

            //compute position
            float3 newPos = (float3) currRotation* 10;
            //Debug.Log(newPos);
            //Debug.Log("gen cube");
            //set the entities component data
            //use currRotation to set position.
            em.SetComponentData(cubeEntities[cubeIdx], new Position { Value = newPos });

            //em.SetComponentData(cubeEntities[cubeIdx], new TransformParent { Value = parentEntity });


        }


        public void GenerateSpawn(int spawnIdx, Vector3 currRotation, Entity parentEntity)
        {
            spawnEntities[spawnIdx] = em.Instantiate(spawnPrefab); //instantiate entity

            //in spawn
            //Debug.Log("in spawn");
            //Debug.Log(currIter);

            //compute position
            float3 newPos = (float3)currRotation * 10;

            //set the entities component data
            //use currRotation to set position.
            em.SetComponentData(spawnEntities[spawnIdx], new Position { Value = newPos });
           // em.SetComponentData(spawnEntities[spawnIdx], new TransformParent { Value = parentEntity });


        }


        public void GenerateRing(int numPartitions, int maxAngle, Vector3 currRotation, bool isSpawn, Entity parentEntity)
        {
            //Vector3 currRotation = new Vector3(0,0,0);
            //set a new currRotation
            //Debug.Log(numPartitions);
            //Debug.Log(maxAngle);
            int increment = maxAngle/numPartitions;
            float3[] positions = new float3[numPartitions];

            for (int i=0; i< numPartitions; i++)
            {
                //instantiate an entity
                if (isSpawn)
                {
                    GenerateSpawn(spawnCounter, currRotation, parentEntity);

                    //then recursively call init spawns and pass in a position
                    if (currIter < maxIter)
                    {
                        currIter = currIter + 1;

                        //Debug.Log("gen new system");
                        GenerateSystem(ringsPerSystem, 180, true, spawnEntities[spawnCounter]); //if not at max depth then gen a spawn system

                    }
                    else
                    {
                        GenerateSystem(ringsPerSystem, 180, false, cubeEntities[cubeCounter]); //else generate cube system
                        currIter = currIter - 1; //I think this is where I reset the iter. Is it 0 or 1?
                                      //reset other variables here
                    }


                    spawnCounter = spawnCounter + 1;
                }
                else
                {
                    GenerateCube(cubeCounter, currRotation, parentEntity);
                    cubeCounter = cubeCounter + 1;
                }




                //currIter = currIter + 1;
                //spawnCounter = spawnCounter + 1;

                //if (currIter = 1)
                //{

                //}


            }

            currIter = currIter - 1; //decrement when reaching end of level
            //Debug.Log(increment);
            currRotation = Quaternion.AngleAxis(increment, Vector3.forward) * currRotation; //increment the angle
            Debug.Log(currRotation);
        }


        //Generates a system by spawning a set of rings.
        //  The rings spawn a set of systems
        public void GenerateSystem(int numPartitions, int maxAngle, bool isSpawn, Entity parentEntity)
        {
            Debug.Log("in system");
            Vector3 currRotation = new Vector3(1, 1, 1);

            int increment = maxAngle / numPartitions;
            //float3[] positions = new float3[numPartitions];

            for (int i = 0; i < numPartitions; i++)
            {

                //gen a ring
                GenerateRing(spawnsPerRing, 360, currRotation, isSpawn, parentEntity);


                //vector = Quaternion.AngleAxis(-45, Vector3.up) * vector;

                currRotation = Quaternion.AngleAxis(increment, Vector3.up) * currRotation; //increment the angle
            }
            currIter = currIter - 1; //decrement when reaching end of level

        }

        private void OnDisable()
        {
            spawnEntities.Dispose();
            cubeEntities.Dispose();
            worldEntity.Dispose();
        }




        /*
        //return an array of Vector3 points describing locations of objects on ring
        public Vector3[] GenerateElementPositions(int increment, int maxAngle, Vector3 rotAxis)
        {
            //t.rotation.Set(0, 0, 0, 0); //reset rotation
            int numElements = maxAngle / increment;
            Vector3[] positions = new Vector3[numElements];

            for (int i = 0; i < (maxAngle / increment); i++)
            {
                Debug.Log(t.rotation);
                t.Rotate(rotAxis * increment);
                Vector3 point = t.forward;
                //Vector3 point = t.rotation * new Vector3(0, 0, 1);
                //Debug.Log(t.rotation);

                positions[i] = point;
            }
            //t.rotation.Set(0, 0, 0, 0); //reset rotation
            return positions;
        }
        */

        /*
        public void GenRing(int elementIncrement, int maxAngle, Vector3 rotAxis, int radius)
        {
            Vector3[] positions = posHelper.GenerateElementPositions(elementIncrement, maxAngle, rotAxis);

            elementHolder = new GameObject[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                //Debug.Log(positions[i]);
                elementHolder[i] = (GameObject)Instantiate(element, gameObject.transform);
                elementHolder[i].GetComponent<Transform>().position = positions[i] * radius;
            }
            //elementHolder;
        }
        */



        /*
        //this function creates a single world entity that spawns all object systems and subsystem and subsubsystems, etc.
        void InitWorld(int dummyNumSystems)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(1, Allocator.Temp);

            em.Instantiate(worldSystem, entities);

            em.SetComponentData(entities[0], new Position { Value = new float3(0, 0, 0) });
            em.SetComponentData(entities[0], new NumberOfSystems { Value = dummyNumSystems }); //set number of subsystems contained by this entity
            //set the number of levels of detail 

            entities.Dispose(); //clear from memory

        }
        */

        /*
        void AddCubes(int amount)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(amount, Allocator.Temp);
            em.Instantiate(cubePrefab, entities);

            for (int i = 0; i < amount; i++)
            {
                float xVal = Random.Range(-100f, 100f);
                float zVal = Random.Range(-100f, 100f);
                em.SetComponentData(entities[i], new Position { Value = new float3(xVal, 0f, zVal) });
                em.SetComponentData(entities[i], new Rotation { Value = new quaternion(0, 1, 0, 0) });
                em.SetComponentData(entities[i], new MoveSpeed { speed = 1 });
            }
            entities.Dispose();
        }
        */


        // Update is called once per frame
        void Update ()
        {
            //if (Input.GetKeyDown("space"))
            //{
            //    AddCubes(1000);
            //}

            /*
//now create the cubes around a given parent position
void InitCubes(float3 parentPosition)
{

GenerateSystem(ringsPerSystem, 180, maxIter, false);

for (int i = 0; i < ringsPerSystem; i++)
{
for (int j = 0; j < cubesPerRing; i++)
{
    //do the following
    cubeEntities[cubeCounter] = em.Instantiate(cubePrefab);

    //set the entity component data
    em.SetComponentData(cubeEntities[i], new Position { Value = new float3(xVal, 0f, zVal) });


    //iterate
    cubeCounter = cubeCounter + 1;



}
}
}
*/

        }
    }
}

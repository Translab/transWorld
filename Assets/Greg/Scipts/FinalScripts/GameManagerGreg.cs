using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Greg
{
    public class GameManagerGreg : MonoBehaviour
    {

        public static GameManagerGreg instance = null;
        EntityManager em;


        //init parameters
        //public int maxIter;
        //private int currIter;
        //private int currDrawDepth;
        public Vector3 worldOrigin;
        public int ringsPerSystem;
        public int spawnsPerRing;
        public int cubesPerRing;

        //public GameObject cubePrefab;
        //public GameObject spawnPrefab;
        public GameObject worldPrefab;
        public EntityArchetype attachArchetype;
        public GameObject blackHoleSpawnPrefab;

        //private int spawnsPerSystem;
        //private int cubesPerSystem;

        //private int spawnCounter;
        //private int cubeCounter;

        //private NativeArray<Entity> spawnEntities; //holds all spawn points
        //private NativeArray<Entity> cubeEntities; //holds all cube entities
        private NativeArray<Entity> worldEntity; //parent to everything, is root


        //public float stuff1 = 1;
        //public float stuff2 = 1;
        //public float moveSpeed = 1;


        void Awake()
        {
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

            //create all archetypes
            attachArchetype = em.CreateArchetype(typeof(Attach));

        }

        void Start()
        {
            Debug.Log("Game Manager start");

            InitWorld();
        }

        void InitWorld()
        {
            worldEntity = new NativeArray<Entity>(1, Allocator.Persistent); //world entity
            worldEntity[0] = em.Instantiate(worldPrefab); //instantiate world entity

            //new spawn info
            //var newSpawn = new SpawnNewSystem
            //{
            //    prefab = spawnPrefab,
                //spawnLocal = true,
                //radius = 10,
            //    systemDepth = 0,
            //    maxDepth = 1,
            //    isSpawning = true
            //};

            em.SetComponentData(worldEntity[0], new Position { Value = worldOrigin });

            //em.SetSharedComponentData(worldEntity[0], newSpawn);
            worldEntity.Dispose();

            ///spawnEntities = new NativeArray<Entity>(1, Allocator.Persistent); //world entity
            //spawnEntities[0] = em.Instantiate(spawnPrefab); //instantiate world entity

            //em.SetComponentData(spawnEntities[0], new Position { Value = new float3(2, 2, 2) });

            //spawnEntities.Dispose();

            //spawnsPerSystem = ringsPerSystem * spawnsPerRing;
            //cubesPerSystem = ringsPerSystem * cubesPerRing;

            //int numSpawns = ComputeNumSpawns(spawnsPerSystem, maxIter); //get total number of spawn entities
            //int numCubes = ComputeNumCubes(spawnsPerSystem, cubesPerSystem, maxIter); //get total number of cubes
            //spawnEntities = new NativeArray<Entity>(numSpawns, Allocator.Persistent); //preallocate spawn entity arrays
            //cubeEntities = new NativeArray<Entity>(numCubes, Allocator.Persistent); //preallocate cube entity arrays
            //worldEntity = new NativeArray<Entity>(1, Allocator.Persistent); //world entity

            //set init variables
            //currIter = 1;
            //cubeCounter = 0;
            //spawnCounter = 0;

            //worldEntity[0] = em.Instantiate(worldPrefab); //instantiate world entity
            //GenerateSystem(ringsPerSystem, 180, true, worldEntity[0]); //start the recursive process

        }

        //dispose the entity when the game is disabled (so no memory leaks)
        private void OnDisable()
        {
            //worldEntity.Dispose();
        }

    }
}


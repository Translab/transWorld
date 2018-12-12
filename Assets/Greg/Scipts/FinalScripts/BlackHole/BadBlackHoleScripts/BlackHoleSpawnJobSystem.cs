using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Samples.Common;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;


namespace Greg
{

}
    /*
    public class SpawnBarrier: BarrierSystem
    {

    }

    public class BlackHoleSpawnJobSystem : JobComponentSystem
    {
        public EntityArchetype attachArch; //for parenting
        public EntityArchetype spawnArch; //entity to spawn

        public struct BlackHoleGroup
        {
            public ComponentDataArray<Position> Positions;
            public ComponentDataArray<BlackHoleCount> BlackHoleCounts;
            public ComponentDataArray<BlackHoleSpawn> BlackHoleSpawn;
            public EntityArray Entities;
            public readonly int Length;
        }

        [Inject] private BlackHoleGroup m_Group;

        [BurstCompile]
        struct BlackHoleSpawnJob : IJobParallelFor
        {
            public ComponentDataArray<Position> sourcePositions;
            public ComponentDataArray<BlackHoleCount> counts;
            public ComponentDataArray<BlackHoleSpawn> blackHoleSpawner;
            public EntityArray sourceEntities;

            //also need the init component to grab prefab and stuff for instantiation
            //public ComponentDataArray<>

            public void Execute(int i)
            {
                var diff = blackHoleSpawner[i].maxCount - counts[i].count;

                //if below the max count of black hole particles then do something
                if (diff > 0)
                {
                    var center = sourcePositions[i].Value;
                    var sourceEntity = sourceEntities[i];
                    var prefab = blackHoleSpawner[i].prefab;
                    float radius = blackHoleSpawner[i].radius;
                    //float3 center = positions[i].Value;
                    //float spawnerCount = blackHole.radius;
                    //float3 center = positions
                    //float3 center = [i].Value;

                    //Debug.Log(diff);
                    //attachArch = GameManagerGreg.instance.attachArchetype;
                    //m_Group.

                    var entities = new NativeArray<Entity>(diff, Allocator.Temp); //create array to hold entities
                    var positions = new NativeArray<float3>(diff, Allocator.Temp);
                    //EntityManager.SetComponentData(sourceEntity, new BlackHoleCount { count = spawner.maxCount });

                    //var sharedBlackHoleCount = new BlackHoleCount
                    //{
                    //    count = blackHole.maxCount
                    //};
                    //EntityManager.SetComponentData(sourceEntity, new BlackHoleCount { count = blackHoleSpawner[i].maxCount});
                    //PostUpdateCommand.
                    //EntityCommandBuffer.c
                    GeneratePoints.RandomPointsOnSphere(center, radius, ref positions);
                    //EntityCommandBuffer
                    //float3 center = spawnInstances[spawnIndex].position;
                    //var sourceEntity = spawnInstances[spawnIndex].sourceEntity;
                    
                    EntityManager.Instantiate(prefab, entities); //instantiate all necessary entities
                    Debug.Log(diff);
                    for (int j = 0; j < diff; j++) //for each entity - set positions and acceleration and heading
                    {
                        //gen set of points on sphere


                        Debug.Log("gening new thing");
                        var position = new Position
                        {
                            Value = positions[j]
                        };
                        EntityManager.SetComponentData(entities[j], position);


                        //Debug.Log("gening new thing");

                        Vector3 normAccel = ((Vector3)positions[j] * -1).normalized; //get norm accel
                        float3 newAccel = (normAccel * UnityEngine.Random.Range(1f, 2f)); //randomize accel

                        var blackHoleChild = new BlackHoleChild
                        {
                            parent = sourceEntity,
                            acceleration = newAccel,
                            velocity = new float3(0, 0, 0),
                            destroyRadius = 10 //arbitrary for right now
                        };
                        EntityManager.SetComponentData(entities[j], blackHoleChild);


                        var scaleSpeed = new BlackHoleScaleSpeed
                        {
                            Value = UnityEngine.Random.Range(0.1f, 0.2f)
                        };
                        EntityManager.SetComponentData(entities[j], scaleSpeed);


                        var localScale = new Scale
                        {
                            Value = new float3(0, 0, 0)
                        };
                        EntityManager.SetComponentData(entities[j], localScale);

                        //EntityManager.AddSharedComponentData(entities[j], sharedBlackHoleCount);


                        //EntityManager.SetComponentData(entities[(i * spawner.objsPerRing) + j], new MoveSpeed { speed = UnityEngine.Random.Range(-.1f, .1f) });


                        // var attachArch = GameManagerGreg.instance.attachArchetype;

                        var attachEntity = EntityManager.CreateEntity(attachArch);
                        
                        EntityManager.SetComponentData(attachEntity, new Attach
                        {
                            Child = entities[j],
                            Parent = sourceEntity
                        });

                    }
                    positions.Dispose();
                    entities.Dispose();
                }


            }

                //var currCount = ComponentDataArray<>


            }

        }
        
        protected override JobHandle OnUpdate(JobHandle inputsDeps)
        {
            var spawnJob = new BlackHoleSpawnJob();
            spawnJob.positions = m_Group.Positions;
            spawnJob.counts = m_Group.BlackHoleCounts;
            spawnJob.blachHoleSpawn = m_Group.BlackHoleSpawn;

        return spawnJob.Schedule(m_Group.Length, 64, inputsDeps);
        }
    }
}
        /*
        public struct Group
        {
            // ComponentDataArray lets us access IComponentData 
            public ComponentDataArray<Position> Positions;

            // ComponentArray lets us access any of the existing class Component                
            //public SharedComponentDataArray<BlackHole> BlackHoles;
            public ComponentDataArray<BlackHoleCount> BlackHoleCounts;


            // Sometimes it is necessary to not only access the components
            // but also the Entity ID.
            public EntityArray Entities;
            public int Length;

        }
        */



        /*
        public EntityArchetype attachArch;


        struct BlackHoleInstance
        {
            public int spawnerIndex;
            public Entity sourceEntity;
            public float3 position;
            //public int myCount;
            public int maxCount;
#pragma warning disable 649
            public float radius;
#pragma warning restore 649
        }

        ComponentGroup m_MainGroup;


        /*
        struct BlackHoleSpawnInstance
        {
            public int spawnerIndex;
            public Entity sourceEntity;
            public float3 position;
#pragma warning disable 649
            public float radius;
#pragma warning restore 649
        }

        [Inject] ComponentGroup m_MainGroup;
        */
        /*
        protected override void OnCreateManager()
        {
            m_MainGroup = GetComponentGroup(typeof(Position), typeof(BlackHoleCount), typeof(BlackHole), typeof(InitBlackHoleSpawn));
        }

        protected override void OnUpdate()
        {

            //EntityManager.GetAllUniqueSharedComponentData<>(BlackHole);

            var uniqueTypes = new List<BlackHole>();
            var sharedIndices = new List<int>();
            EntityManager.GetAllUniqueSharedComponentData(uniqueTypes, sharedIndices);

            //var uniqueTypes = new List<BlackHole>(10); //make ten unique type? 
            attachArch = GameManagerGreg.instance.attachArchetype;

            EntityManager.GetAllUniqueSharedComponentData(uniqueTypes);
            //bc shared component data may have non-blittable types and thus need a list not a nativeContainer

            int spawnInstanceCount = 0; //how many spawn instances are there?
            //Debug.Log("on Update");
            //Debug.Log(uniqueTypes.Count);
            for (int sharedIndex = 0; sharedIndex != uniqueTypes.Count; sharedIndex++)
            {
                //Debug.Log(sharedIndex);
                var spawner = uniqueTypes[sharedIndex];
                m_MainGroup.SetFilter(spawner);
                var sourceEntities = m_MainGroup.GetEntityArray();
                //Debug.Log(sourceEntities.Length);

                spawnInstanceCount += sourceEntities.Length;
            } //i'm not sure what this does....?
            //Debug.Log(spawnInstanceCount);

            if (spawnInstanceCount == 0) //if no spawn instances then stop
                return;

            //instantiate an spawnInstance
            var spawnInstances = new NativeArray<BlackHoleInstance>(spawnInstanceCount, Allocator.Temp); //native array of components
            {
                int spawnIndex = 0;
                for (int sharedIndex = 0; sharedIndex != uniqueTypes.Count; sharedIndex++) //for each
                {
                    var spawner = uniqueTypes[sharedIndex];
                    m_MainGroup.SetFilter(spawner);
                    var sourceEntities = m_MainGroup.GetEntityArray();
                    var positions = m_MainGroup.GetComponentDataArray<Position>();
                    //var counts = m_MainGroup.GetComponentDataArray<BlackHoleCount>();
                    //var radii = m_MainGroup.GetComponentDataArray<RingRadius>();

                    //this is what needs to change
                    for (int entityIndex = 0; entityIndex < sourceEntities.Length; entityIndex++)
                    {
                        var spawnInstance = new BlackHoleInstance();

                        spawnInstance.sourceEntity = sourceEntities[entityIndex];
                        spawnInstance.spawnerIndex = sharedIndex;
                        spawnInstance.position = positions[entityIndex].Value;
                        //spawnInstance.myCount = counts[entityIndex].count;
                        spawnInstance.maxCount = spawner.maxCount;

                        //spawnInstance.radius = radii[entityIndex].Value;

                        spawnInstances[spawnIndex] = spawnInstance;
                        spawnIndex++;
                    }
                }
            }



            //Debug.Log("in black hole spawn system");
            //var sourceEntities = m_MainGroup.GetEntityArray();
            //var blackHoles = m_MainGroup.GetSharedComponentDataArray<BlackHole>();
            //var blackHoleCounts = m_MainGroup.GetComponentDataArray<BlackHoleCount>();
            //var sourcePositions = m_MainGroup.GetComponentDataArray<Position>();
            
            //attachArch = EntityManager.CreateArchetype(typeof(Attach));
            for (int spawnIndex = 0; spawnIndex < spawnInstances.Length; spawnIndex++)
            {
                //get diff

                //grab this sourceEntities stuff
                //var spawnerCounts = blackHoleCounts[i];
                //var spawner = blackHoles[i];
                //var prefab = spawner.prefab;
                //float radius = spawner.radius;
                //float3 center = sourcePositions[i].Value; //get center

                //var entities = new NativeArray<Entity>(diff, Allocator.Temp);


                //add more stuff

                //EntityManager.Instantiate(prefab, entities);

                // Iterate over all entities matching the declared ComponentGroup required types
                //for (int i = 0; i != sourceEntities.Length ; i++)
                //{

                      //              var sourceEntity = m_Group.BlackHoleChildrenInfo[i].parent;
                    //var currCount = EntityManager.GetComponentData(sourceEntity); //get curr count
                    //var currCount = EntityManager.GetComponentData<BlackHoleCount>(sourceEntity).count; //get curr count
                    //EntityManager.SetComponentData<BlackHoleCount>(sourceEntity, new BlackHoleCount { count = currCount - 1 }); //decrement the counr
                    //C
                Debug.Log("in black hole spawn system");
                var sourceEntity = spawnInstances[spawnIndex].sourceEntity;
                var currCount = EntityManager.GetComponentData<BlackHoleCount>(sourceEntity).count; //get curr count
                int spawnerIndex = spawnInstances[spawnIndex].spawnerIndex;

                var spawner = uniqueTypes[spawnerIndex];
                //var counts = m_MainGroup.GetComponentDataArray<BlackHoleCount>();
                var diff = spawner.maxCount - currCount; //get diff

                if (diff > 0) //if not enough pieces then spawn idff
                {
                    var prefab = spawner.prefab;
                    float radius = spawner.radius;
                    //float spawnerCount = spawner.radius;
                    float3 center = spawnInstances[spawnIndex].position;

                    Debug.Log(diff);
                    //attachArch = GameManagerGreg.instance.attachArchetype;
                    //m_Group.

                    var entities = new NativeArray<Entity>(diff, Allocator.Temp); //create array to hold entities
                    var positions = new NativeArray<float3>(diff, Allocator.Temp);
                    //EntityManager.SetComponentData(sourceEntity, new BlackHoleCount { count = spawner.maxCount });

                    var sharedBlackHoleCount = new BlackHoleCount
                    {
                        count = spawner.maxCount
                    };
                    EntityManager.SetComponentData(sourceEntity, sharedBlackHoleCount);


                    GeneratePoints.RandomPointsOnSphere(center, radius, ref positions);

                    //float3 center = spawnInstances[spawnIndex].position;
                    //var sourceEntity = spawnInstances[spawnIndex].sourceEntity;

                    EntityManager.Instantiate(prefab, entities); //instantiate all necessary entities
                    Debug.Log(diff);
                    for (int j = 0; j < diff; j++) //for each entity - set positions and acceleration and heading
                    {
                        //gen set of points on sphere


                        //Debug.Log("gening new thing");
                        var position = new Position
                        {
                            Value = positions[j]
                        };
                        EntityManager.SetComponentData(entities[j], position);


                        //Debug.Log("gening new thing");

                        Vector3 normAccel = ((Vector3)positions[j] * -1).normalized; //get norm accel
                        float3 newAccel = (normAccel * UnityEngine.Random.Range(1f, 2f)); //randomize accel

                        var blackHoleChild = new BlackHoleChild
                        {
                            parent = sourceEntity,
                            acceleration = newAccel,
                            velocity = new float3(0,0,0),
                            destroyRadius = 10 //arbitrary for right now
                        };
                        EntityManager.SetComponentData(entities[j], blackHoleChild);


                        var scaleSpeed = new BlackHoleScaleSpeed
                        {
                            Value = UnityEngine.Random.Range(0.1f,0.2f)
                        };
                        EntityManager.SetComponentData(entities[j], scaleSpeed);


                        var localScale = new Scale
                        {
                            Value = new float3(0, 0, 0)
                        };
                        EntityManager.SetComponentData(entities[j], localScale);

                        //EntityManager.AddSharedComponentData(entities[j], sharedBlackHoleCount);


                        //EntityManager.SetComponentData(entities[(i * spawner.objsPerRing) + j], new MoveSpeed { speed = UnityEngine.Random.Range(-.1f, .1f) });


                        // var attachArch = GameManagerGreg.instance.attachArchetype;

                        var attachEntity = EntityManager.CreateEntity(attachArch);
                        
                        EntityManager.SetComponentData(attachEntity, new Attach
                        {
                            Child = entities[j],
                            Parent = sourceEntity
                        });
                        
                      

                        //var attachEntity = EntityManager.CreateEntity(attachArch);
                        /*
                        EntityManager.SetComponentData(attachEntity, new Attach
                        {
                            Child = entities[j],
                            Parent = sourceEntities[i]
                        });
                        */

                        // Spawn Attach
                        /*
                        var attach = EntityManager.CreateEntity();
                        EntityManager.AddComponentData(attach, new Attach
                        {
                            Parent = sourceEntities[i],
                            Child = entities[j]
                        });
                        */
                        /*
                    }
                    positions.Dispose();
                    entities.Dispose();
                    
                    EntityManager.RemoveComponent<InitBlackHoleSpawn>(sourceEntity);

                }

                //sourceEntities.Dispose();

            }
            spawnInstances.Dispose();


        }
    }
}
*/
             




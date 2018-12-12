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

    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;
    using Samples.Common;
    using UnityEngine;


    namespace Greg
    {

        public class InitBlackHoleSystem : ComponentSystem
        {

            public EntityArchetype attachArch;
            public GameObject prefab;
            private int totalCount;

            struct InitBlackHoleGroup
            {

                public readonly int Length;
                [ReadOnly] public ComponentDataArray<Position> Positions;
                [ReadOnly] public EntityArray Entities;
                [ReadOnly] public ComponentDataArray<BlackHole> BlackHoles;
                [ReadOnly] public ComponentDataArray<InitBlackHoleSpawn> initSpawn;
            }

            [Inject] InitBlackHoleGroup m_Group;


            protected override void OnUpdate()
            {
                attachArch = GameManagerGreg.instance.attachArchetype; //get archetype for parenting 
                                                                       //m_MainGroup = GetComponentGroup(typeof(Position), typeof(BlackHoleCount), typeof(BlackHole));
                prefab = GameManagerGreg.instance.blackHoleSpawnPrefab;

                totalCount = 0;

                //int totalCount;
                //for (int i=0; i< m_Group.Length; i++)
                //{
                //    totalCount = totalCount + m_Group.BlackHoles[i].maxCount;
                //}
                

                //GeneratePoints.RandomPointsOnSphere(center, radius, ref newPositions); //generate all init positions
                

                //int n =0 ;

                
                for (int i = 0; i < m_Group.Length; i++)
                {

                    var blackHole = m_Group.BlackHoles[i];
                    var sourceEntity = m_Group.Entities[i];

                    float radius = blackHole.radius /5;
                    float3 center = m_Group.Positions[i].Value;
                    Debug.Log(radius);

                    var dummyPositions = new float3[blackHole.maxCount];

                    dummyPositions = GeneratePoints.RandomPointsOnSphereFloat3(center, radius, dummyPositions); //generate all init positions

                    for (int j = 0; j< blackHole.maxCount; j++)
                    {

                        var newEntity = new NativeArray<Entity>(1, Allocator.TempJob); //create array to hold entities
                                //var newPositions = new NativeArray<float3>(1, Allocator.TempJob);
                        EntityManager.Instantiate(prefab, newEntity); //instantiate all necessary entities


                        var childPosition = new Position
                        {
                            Value = dummyPositions[j]
                        };
                        //EntityManager.SetComponentData(entities[n], childPosition);
                        EntityManager.SetComponentData(newEntity[0], childPosition);


                        Vector3 normAccel = ((Vector3)dummyPositions[j] * -1).normalized; //get norm accel
                        float3 newAccel = (normAccel * UnityEngine.Random.Range(1f, 2f)); //randomize accel
                        var blackHoleChild = new BlackHoleChild
                        {
                            parent = sourceEntity,
                            acceleration = newAccel,
                            velocity = new float3(0, 0, 0),
                            spawnRadius = radius,
                            destroyRadius = .1f //arbitrary for right now
                        };
                        EntityManager.SetComponentData(newEntity[0], blackHoleChild);


                        var scaleSpeed = new BlackHoleScaleSpeed
                        {
                            Value = UnityEngine.Random.Range(0.1f, 0.2f)
                        };
                        EntityManager.SetComponentData(newEntity[0], scaleSpeed);


                        var localScale = new Scale
                        {
                            Value = new float3(0, 0, 0)
                        };
                        EntityManager.SetComponentData(newEntity[0], localScale);


                        var attachEntity = EntityManager.CreateEntity(attachArch);
                        EntityManager.SetComponentData<Attach>(attachEntity, new Attach
                        {
                            Child = newEntity[0],
                            Parent = sourceEntity
                        });

                        newEntity.Dispose();
                        //n++;
                    }                  
                    PostUpdateCommands.RemoveComponent<InitBlackHoleSpawn>(sourceEntity);
                   
                }
                
                Debug.Log("done spawning");
                //entities.Dispose();
                //newPositions.Dispose();
            }
        }
    }
}

    /*
    //for deleting entities
    public class OutputBarrier : BarrierSystem
    {
    }

    //for decrementing the count
    public class SetComponentBarrier : BarrierSystem
    {
    }

    public class InitBlackHoleSystem : JobComponentSystem
    {

        public EntityArchetype attachArch;
        public EntityArchetype spawnArch;

        struct InitBlackHoleGroup
        {

            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Position> Positions;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<BlackHoleSpawn> BlackHoleSpawns;
            //[ReadOnly]
            //public SharedComponentDataArray<BlackHoleCount> count;
        }


        [Inject] InitBlackHoleGroup m_Group;
        [Inject] private OutputBarrier m_Barrier;
        //[Inject] private DecrementCountBarrier m_countBarrier;

        [BurstCompile]
        struct InitBlackHoleJob : IJob
        {
            [ReadOnly] public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<BlackHoleSpawn> blackHoleSpawns;
            [ReadOnly] public EntityArray entities; //check if this needs to be writing too? 
            [WriteOnly] public EntityCommandBuffer buff; //write to buffer

            [ReadOnly] public EntityArchetype attachArch;
            [ReadOnly] public EntityArchetype spawnArch;

            public void Execute()
            {


      
                //Debug.Log(diff);
                //attachArch = GameManagerGreg.instance.attachArchetype;
                //m_Group.

                //var entities = new NativeArray<Entity>(diff, Allocator.Temp); //create array to hold entities
                //var positions = new NativeArray<float3>(diff, Allocator.Temp);
                //EntityManager.SetComponentData(sourceEntity, new BlackHoleCount { count = spawner.maxCount });

                //var sharedBlackHoleCount = new BlackHoleCount
                //{
                //    count = blackHole.maxCount
                //};
                //EntityManager.SetComponentData(sourceEntity, sharedBlackHoleCount);


                for (int i = 0; i < positions.Length; i++)
                {

                    //generate arrays
                    //var spawnEntities = new NativeArray<Entity>(blackHoleSpawns[i].maxCount, Allocator.Temp);
                    var spawnPositions = new NativeArray<float3>(blackHoleSpawns[i].maxCount, Allocator.Temp);

                    var sourceEntity = entities[i];
                    var prefab = blackHoleSpawns[i].prefab;
                    var radius = blackHoleSpawns[i].radius;
                    var center = blackHoleSpawns[i].center;

                    //get particle positions
                    GeneratePoints.RandomPointsOnSphere(center, radius, ref positions);

                    EntityManager.Instantiate(prefab, entities); //instantiate all necessary entities
                    Debug.Log(diff);
                    for (int j = 0; j < diff; j++) //for each entity - set positions and acceleration and heading
                    {
                        //gen set of points on sphere
                        buff.CreateEntity(Attach)
                        buff.AddComponent(new Position { Value = new float3(0, 0, 0) });
                        buff.SetSharedComponent()

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



                //do stuff
                half distance = ((Vector3)positions[i].Value).magnitude; //get distance from center of spawner (bc children are parented)
                    if (distance < blackHoleChildrenInfo[i].destroyRadius)
                    {
                        var sourceEntity = blackHoleChildrenInfo[i].parent;

                        deleteBuffer.DestroyEntity(entities[i]); //destory an entity
                                                                 //countBuffer.AddComponent(sourceEntity, new DecrementBlackHoleCount { }); //tell parent to spawn a new one

                    }

                    //buff.RemoveComponent
                }

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputsDeps)
        {
            var initJob = new InitBlackHoleJob();
            initJob.positions = m_Group.Positions;
            initJob.entities = m_Group.Entities;
            initJob.blackHoleSpawns = m_Group.BlackHoleSpawns;
            initJob.buff = m_Barrier.CreateCommandBuffer();
            return initJob.Schedule(inputsDeps);
        }
    }
}

    */

using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Samples.Common;
using UnityEngine;


namespace Greg
{
}
    /*
    public class BlackHoleRespawnSystem : ComponentSystem
    {
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

        /*
        struct BlackHoleRespawnGroup
        {

            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Position> Positions;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly]
            public SharedComponentDataArray<BlackHole> BlackHoles;
            public ComponentDataArray<BlackHoleCount> BlackHolesCounts;
        }
        */
        /*
        struct BlackHoleRespawnGroup
        {

            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Position> Positions;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<BlackHole> BlackHoles;
            [ReadOnly] public ComponentDataArray<AddBlackHoleSpawn> toSpawn;
        }

        [Inject] BlackHoleRespawnGroup m_Group;

        protected override void OnCreateManager()
        {
            attachArch = GameManagerGreg.instance.attachArchetype; //get archetype for parenting 
            //m_MainGroup = GetComponentGroup(typeof(Position), typeof(BlackHoleCount), typeof(BlackHole));
        }

        protected override void OnUpdate()
        {

            //EntityManager.GetAllUniqueSharedComponentData<>(BlackHole);

            //var uniqueTypes = new List<BlackHole>();
            //var sharedIndices = new List<int>();
            //EntityManager.GetAllUniqueSharedComponentData(uniqueTypes, sharedIndices);

            //var uniqueTypes = new List<BlackHole>(10); //make ten unique type? 

            //attachArch = GameManagerGreg.instance.attachArchetype;

            var entities = new NativeArray<Entity>(m_Group.Length, Allocator.Temp); //create array to hold entities
            EntityManager.Instantiate(m_Group.BlackHoles[0].prefab, entities); //instantiate all necessary entities

            for (int i = 0; i < m_Group.Length; i++)
            {
                var blackHole = m_Group.BlackHoles[i];

                var sourceEntity = m_Group.Entities[i];
                var prefab = blackHole.prefab;
                float radius = blackHole.radius;

                float3 center = m_Group.Positions[i].Value;

                var localPosition = new NativeArray<float3>(1, Allocator.Temp);

                GeneratePoints.RandomPointsOnSphere(center, radius, ref localPosition);

                var newPosition = new Position
                {
                    Value = localPosition[0]
                };
                EntityManager.SetComponentData(entities[i], newPosition);


                //Debug.Log("gening new thing");

                Vector3 normAccel = ((Vector3)localPosition[0] * -1).normalized; //get norm accel
                float3 newAccel = (normAccel * UnityEngine.Random.Range(1f, 2f)); //randomize accel

                var blackHoleChild = new BlackHoleChild
                {
                    parent = sourceEntity,
                    acceleration = newAccel,
                    velocity = new float3(0, 0, 0),
                    destroyRadius = 10 //arbitrary for right now
                };
                EntityManager.SetComponentData(entities[i], blackHoleChild);


                var scaleSpeed = new BlackHoleScaleSpeed
                {
                    Value = UnityEngine.Random.Range(0.1f, 0.2f)
                };
                EntityManager.SetComponentData(entities[i], scaleSpeed);


                var localScale = new Scale
                {
                    Value = new float3(0, 0, 0)
                };
                EntityManager.SetComponentData(entities[i], localScale);

                //EntityManager.AddSharedComponentData(entities[j], sharedBlackHoleCount);


                //EntityManager.SetComponentData(entities[(i * spawner.objsPerRing) + j], new MoveSpeed { speed = UnityEngine.Random.Range(-.1f, .1f) });


                // var attachArch = GameManagerGreg.instance.attachArchetype;

                var attachEntity = EntityManager.CreateEntity(attachArch);

                EntityManager.SetComponentData(attachEntity, new Attach
                {
                    Child = entities[i],
                    Parent = sourceEntity
                });

                //EntityManager.RemoveComponent()
                localPosition.Dispose();
            }
            entities.Dispose();


            //now clean up components
            //EntityManager.RemoveComponent<>

        }
    }
}



                /*
                //Debug.Log(diff);
                for (int j = 0; j < diff; j++) //for each entity - set positions and acceleration and heading
                {
                    //gen set of points on sphere


                    Debug.Log("gening new thing");
                    

                }
                positions.Dispose();
                entities.Dispose();
            }
        }



        //var sourceEntity = m_Group.sourceEntity;
        //var currCount = m_Group.BlackHolesCounts[i].count; // get curr count for instance
        //var blackHole = m_Group.BlackHoles[i];

        //var currCount = EntityManager.GetComponentData<BlackHoleCount>(sourceEntity).count; //get curr count
        //int spawnerIndex = spawnInstances[spawnIndex].spawnerIndex;

        //var spawner = uniqueTypes[spawnerIndex];
        //var counts = m_MainGroup.GetComponentDataArray<BlackHoleCount>();
        //var diff = blackHole.maxCount - currCount; //get diff

        //if (diff > 0) //if not enough pieces then spawn idff
        //{
        Debug.Log("in respawn");
        */
                

/*


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
                var counts = m_MainGroup.GetComponentDataArray<BlackHoleCount>();
                var diff = spawner.maxCount - currCount; //get diff

                if (diff > 0) //if not enough pieces then spawn idff
                {
                    var prefab = spawner.prefab;
                    float radius = spawner.radius;
                    float spawnerCount = spawner.radius;
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
*/

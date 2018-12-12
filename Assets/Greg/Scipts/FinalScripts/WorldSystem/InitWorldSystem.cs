using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Samples.Common;
using UnityEngine;

namespace Greg
{
    public class InitWorldSystem : ComponentSystem
    {
        public EntityArchetype attachArch;

        struct InitWorldSpawnInstance
        {
            public int spawnerIndex;
            public Entity sourceEntity;
            public float3 position;
#pragma warning disable 649
            public float radius;
#pragma warning restore 649
        }

        ComponentGroup m_MainGroup;

        protected override void OnCreateManager()
        {
            m_MainGroup = GetComponentGroup(typeof(Position), typeof(RootDepth), typeof(InitWorld));
        }

        protected override void OnUpdate()
        {
            var uniqueTypes = new List<InitWorld>(10); //make ten unique type? 
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
                var entities = m_MainGroup.GetEntityArray();
                Debug.Log(entities.Length);

                spawnInstanceCount += entities.Length;
            } //i'm not sure what this does....?
            Debug.Log(spawnInstanceCount);

            if (spawnInstanceCount == 0) //if no spawn instances then stop
                return;

            //instantiate an spawnInstance
            var spawnInstances = new NativeArray<InitWorldSpawnInstance>(spawnInstanceCount, Allocator.Temp); //native array of components
            {
                int spawnIndex = 0;
                for (int sharedIndex = 0; sharedIndex != uniqueTypes.Count; sharedIndex++) //for each
                {
                    var spawner = uniqueTypes[sharedIndex];
                    m_MainGroup.SetFilter(spawner);
                    var entities = m_MainGroup.GetEntityArray();
                    var positions = m_MainGroup.GetComponentDataArray<Position>();
                    //var radii = m_MainGroup.GetComponentDataArray<RingRadius>();


                    for (int entityIndex = 0; entityIndex < entities.Length; entityIndex++)
                    {
                        var spawnInstance = new InitWorldSpawnInstance();

                        spawnInstance.sourceEntity = entities[entityIndex];
                        spawnInstance.spawnerIndex = sharedIndex;
                        spawnInstance.position = positions[entityIndex].Value;
                        //spawnInstance.radius = radii[entityIndex].Value;


                        spawnInstances[spawnIndex] = spawnInstance;
                        spawnIndex++;
                    }
                }
            }

            //init speed of harmonic ratios
            float[] initSpeedArray = new float[4] { 1, 2, 4,8};
            for (int q=0; q < initSpeedArray.Length; q++ )
            {
                initSpeedArray[q] = (initSpeedArray[q] * math.pow(2, -2));
                //initSpeedArray[q] = 1;

            }

            for (int spawnIndex = 0; spawnIndex < spawnInstances.Length; spawnIndex++)
            {
                int spawnerIndex = spawnInstances[spawnIndex].spawnerIndex;
                var spawner = uniqueTypes[spawnerIndex];
                var entities = new NativeArray<Entity>(spawner.ringsPerSystem * spawner.objsPerRing, Allocator.Temp);
                var prefab = spawner.prefab;
                float radius = spawner.radius;
                float3 center = spawnInstances[spawnIndex].position;
                var sourceEntity = spawnInstances[spawnIndex].sourceEntity;

                EntityManager.Instantiate(prefab, entities);

                //vars for settings rings
                float zRot = 0;
                float increment = 180 / spawner.ringsPerSystem; //increment is 2*pi / numPartitions
                float ringIncrement = Mathf.PI * 2.0f / spawner.objsPerRing; //increment is 2*pi / numPartitions


                for (int i=0; i< spawner.ringsPerSystem; i++)
                {
                    float angle = 0;

                    var ringPositions = new NativeArray<float3>(spawner.objsPerRing, Allocator.Temp);

                    Quaternion rotation = Quaternion.Euler(0f, 0f, zRot);

                    GeneratePoints.GenerateWorldRing(center, radius, rotation, ref ringPositions);
                    //zRot = zRot + increment;


                    //for each ring do what?
                    //grab a dummy archetype of rotation. 
                    //set the source rotation. the parent rotation. so sourceEntity
                    //I dont think that'll work bc its only parented after the update in transformupdatesystem

                    //float initZRotSpeed = initSpeedArray[UnityEngine.Random.Range(0, initSpeedArray.Length)];
                    //float initZRotSpeed = 1;
                    float initZRotSpeed = 4 * initSpeedArray[UnityEngine.Random.Range(0, initSpeedArray.Length)];

                    for (int j =0; j< spawner.objsPerRing; j++)
                    {
                        var position = new Position
                        {
                            Value = ringPositions[j]
                        };
                        EntityManager.SetComponentData(entities[(i* spawner.objsPerRing) + j], position);
                        //set position and add in other variables
                        //var rotationSpeed = new RotationSpeed
                        //{
                        //    Value = UnityEngine.Random.Range(-1f, 1f)
                        //};

                        //Debug.Log(rotationSpeed.Value);

                        //EntityManager.SetComponentData(entities[(i * spawner.objsPerRing) + j], rotationSpeed);


                        EntityManager.SetComponentData(entities[(i * spawner.objsPerRing) + j], new MoveSpeed { speed = UnityEngine.Random.Range(-.1f, .1f) });

                        //make the values change depending on depth level

                        //how to set move speed
                        float initMoveSpeed = initSpeedArray[UnityEngine.Random.Range(0, initSpeedArray.Length)];


                        var initOffset = new RingRevolve
                        {
                            //center = new float3(0, 0, 0),
                            initOffset = angle,
                            radius = spawner.radius,
                            zRotation = zRot,
                            initSpeed = initMoveSpeed,
                            initRandSpeed = UnityEngine.Random.Range(-2f, 2f),
                            initZRotSpeed = initZRotSpeed,
                            initRandZRotSpeed = UnityEngine.Random.Range(-2f, 2f),
                        };

                        EntityManager.SetComponentData(entities[(i * spawner.objsPerRing) + j], initOffset);
                        angle = angle + ringIncrement;

                        var attachEntity = EntityManager.CreateEntity(attachArch);
                        EntityManager.SetComponentData(attachEntity, new Attach
                        {
                            Child = entities[(i*spawner.objsPerRing) + j],
                            Parent = sourceEntity
                        });
                    }
                    zRot = zRot + increment; //increment z

                    ringPositions.Dispose();

                    //zRot = zRot+increment;
                }

                /*
                for (int i = 0; i < count; i++)
                {
                    var position = new Position
                    {
                        Value = spawnPositions[i]
                    };

                    var attachEntity = EntityManager.CreateEntity(attachArch);
                    //EntityManager.AddComponent(SpawnAttach, new Attach());
                    EntityManager.SetComponentData(attachEntity, new Attach
                    {
                        Child = entities[i],
                        Parent = sourceEntity
                    });

                    EntityManager.SetComponentData(entities[i], position);
                }
                */

                EntityManager.RemoveComponent<InitWorld>(sourceEntity);

                //ringPositions.Dispose();
                entities.Dispose();
            }
            spawnInstances.Dispose();
        }
    }
}

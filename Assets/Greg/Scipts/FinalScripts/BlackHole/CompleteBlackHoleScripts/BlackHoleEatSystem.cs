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

    //for deleting entities
    public class UpdateBarrier : BarrierSystem
    {
    }

    //for decrementing the count
    //public class DecrementCountBarrier: BarrierSystem
    //{
    //}

    public class BlackHoleEatSystem : JobComponentSystem
    {

        struct BlackHoleEatGroup
        {

            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Position> Positions;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<BlackHoleChild> BlackHoleChildrenInfo;
            //[ReadOnly]
            //public SharedComponentDataArray<BlackHoleCount> count;
        }


        [Inject] BlackHoleEatGroup m_Group;
        [Inject] private UpdateBarrier m_Barrier;
        //[Inject] private DecrementCountBarrier m_countBarrier;

        [ComputeJobOptimization]
        struct BlackHoleEatJob : IJob
        {
            [ReadOnly] public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<BlackHoleChild> blackHoleChildrenInfo;
            [ReadOnly] public EntityArray entities; //check if this needs to be writing too? 
            [WriteOnly] public EntityCommandBuffer buff;
            //[WriteOnly] public EntityCommandBuffer countBuffer;

            public float3 randomPosition;
            public float3 randomAcceleration;
            public float randomScaleSpeed;

            public void Execute()
            {
                                
                for (int i = 0; i < positions.Length; i++)
                {
                    //do stuff
                    float distance = ((Vector3) positions[i].Value).magnitude; //get distance from center of spawner (bc children are parented)

                    //Debug.Log(distance);
                    if (distance < blackHoleChildrenInfo[i].destroyRadius)
                    {
                        //reset its values
                        //var newPos = new NativeArray<float3>(1, Allocator.Temp);

                        //float3 newPos = RandomPointOnSphere(new float3(0,0,0), blackHoleChildrenInfo[i].spawnRadius);

                        //var pointsFound = 0;
                        //float3 newPos = new float3(0, 0, 0);
                        //var count = points.Length;
                        /*
                        while (pointsFound < 1)
                        {
                            var p = new Vector3
                            {
                                x = Unity.Mathematics.Random.;
                                //y = UnityEngine.Random.Range(-1f, 1f),
                                //z = UnityEngine.Random.Range(-1f, 1f)
                            };
                            if (math.lengthsq(p) < 1)
                            {
                                p = (p.normalized) * blackHoleChildrenInfo[i].spawnRadius;

                                newPos = (float3) p;
                                pointsFound = 1;
                                //points[pointsFound] = point;

                                //transform to onto circle with radius r


                                //pointsFound++;
                            }
                        }
                        */
                        float3 translatedPosition = randomPosition * blackHoleChildrenInfo[i].spawnRadius;

                        //Debug.Log(randomPosition);

                        var newPosition = new Position
                        {
                            Value = translatedPosition
                        };
                        buff.SetComponent(entities[i], newPosition);


                        Vector3 normAccel = ((Vector3)randomPosition * -1).normalized; //get norm accel
                        float3 newAccel = (normAccel * randomAcceleration); //randomize accel
                        var blackHoleChild = new BlackHoleChild
                        {
                            parent = blackHoleChildrenInfo[i].parent,
                            acceleration = newAccel,
                            velocity = new float3(0, 0, 0),
                            spawnRadius = blackHoleChildrenInfo[i].spawnRadius,
                            destroyRadius = blackHoleChildrenInfo[i].destroyRadius //arbitrary for right now
                        };
                        buff.SetComponent(entities[i], blackHoleChild);


                        var scaleSpeed = new BlackHoleScaleSpeed
                        {
                            Value = randomScaleSpeed //UnityEngine.Random.Range(0.1f, 0.2f)
                        };
                        buff.SetComponent(entities[i], scaleSpeed);


                        var localScale = new Scale
                        {
                            Value = new float3(0, 0, 0)
                        };
                        buff.SetComponent(entities[i], localScale);
                        
                        

                        //deleteBuffer.DestroyEntity(entities[i]); //destory an entity
                        //countBuffer.AddComponent(sourceEntity, new AddBlackHoleSpawn { }); //tell parent to spawn a new one

                    }
                }
                
                
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputsDeps)
        {
            var eatJob = new BlackHoleEatJob();
            eatJob.positions = m_Group.Positions;
            eatJob.entities = m_Group.Entities;
            eatJob.blackHoleChildrenInfo = m_Group.BlackHoleChildrenInfo;
            eatJob.buff = m_Barrier.CreateCommandBuffer();

            //gen random point
            eatJob.randomPosition = GeneratePoints.RandomPointOnSphere();
            eatJob.randomScaleSpeed = UnityEngine.Random.Range(0.1f, 0.2f);
            eatJob.randomAcceleration = UnityEngine.Random.Range(1f, 2f);

            //eatJob.countBuffer = m_countBarrier.CreateCommandBuffer();
            return eatJob.Schedule(inputsDeps);
        }
    }
}

/*
protected override void OnUpdate()
{
    //EntityManager.
    for (int i = 0; i < m_Group.Length; i++)
    {

        //set flag for spawner vs entity
        //need to keep track of different spawners and decrement count

        //int currCount = m_Group;
        float distance = ((Vector3)m_Group.Positions[i].Value).magnitude; //get distance from center of spawner (bc children are parented)
        if (distance < m_Group.BlackHoleChildrenInfo[i].destroyRadius)
        {
            var sourceEntity = m_Group.BlackHoleChildrenInfo[i].parent;
            //var currCount = EntityManager.GetComponentData(sourceEntity); //get curr count
            var currCount = EntityManager.GetComponentData<BlackHoleCount>(sourceEntity).count; //get curr count
            EntityManager.SetComponentData<BlackHoleCount>(sourceEntity, new BlackHoleCount { count = currCount - 1 }); //decrement the counr
            //ComponentDataFromEntity(sourceEntity);
            //EntityManager.ComponentDataFrom
            //var pos = EntityManager.GetComponentData<Position>(m_Group.Entities[i]);
            //m_Group.Entities[i]
            //newCount 
            //PostUpdateCommands.SetSharedComponent(m_Group.Entities[i], new BlackHoleCount { count = m_Group.count[i].count + 1 });
            //PostUpdateCommands.CreateEntity(blackHoleIncrement);
            //emit an entity that is called blackHoleIncrement
            // it has a blackHoleCount component on it as well
            //  in m_group we get component data and check if it 

            PostUpdateCommands.DestroyEntity(m_Group.Entities[i]); //delete
            //
            //PostUpdateCommands.ToConcurrent().SetSharedComponent;
            //EntityCommandBuffer.Concurrent
        }
        //schedule for destruction if within radius
        //HOW TO: schedule addComponents to the spawner for mat changing, etc ? 
    }

}
*/

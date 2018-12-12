using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Samples.Common;

namespace Greg
{
    public class BlackHoleJobSystem : JobComponentSystem
    {
#pragma warning disable 649
        struct BlackHoleChildGroup
        {

            public ComponentDataArray<Position> positions;

            public ComponentDataArray<BlackHoleChild> blackHoleChildren;

            public readonly int Length;
        }

        [Inject] private BlackHoleChildGroup m_BlackHoleChildGroup;
#pragma warning restore 649

        [BurstCompile]
        struct BlackHoleChildPosition : IJobParallelFor
        {
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<BlackHoleChild> blackHoleChildren;
            [ReadOnly]
            public float dt;

            public void Execute(int i)
            {

                //var dir = ((Vector3) positions[i].Value - (Vector3) headings[i].Value).normalized;

                //assume you have direction
                float3 vel = blackHoleChildren[i].velocity + (blackHoleChildren[i].acceleration * dt);
                float3 newPos = positions[i].Value + (vel * dt);


                blackHoleChildren[i] = new BlackHoleChild
                {
                    //t = t, //can remove
                    parent = blackHoleChildren[i].parent,
                    acceleration = blackHoleChildren[i].acceleration,
                    velocity = vel,
                    destroyRadius = blackHoleChildren[i].destroyRadius,
                    spawnRadius = blackHoleChildren[i].spawnRadius
                };

                //float3 newPosition = ringRevolve[i].zRotation * new Vector3(x, y, z);

                positions[i] = new Position
                {
                    Value = newPos
                };
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var blackHoleChildPositionJob = new BlackHoleChildPosition();
            blackHoleChildPositionJob.positions = m_BlackHoleChildGroup.positions;
            blackHoleChildPositionJob.blackHoleChildren = m_BlackHoleChildGroup.blackHoleChildren;
            //blackHoleChildPositionJob.moveSpeeds = m_BlackHoleChildGroup.moveSpeeds;
            blackHoleChildPositionJob.dt = Time.deltaTime;
            return blackHoleChildPositionJob.Schedule(m_BlackHoleChildGroup.Length, 64, inputDeps);
        }
    }
}

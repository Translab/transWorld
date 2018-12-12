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
    public class BlackHoleScaleJobSystem : JobComponentSystem
    {
#pragma warning disable 649
        struct BlackHoleScaleGroup
        {

            public ComponentDataArray<Position> positions;
            public ComponentDataArray<Scale> localScales;
            public ComponentDataArray<BlackHoleScaleSpeed> scaleSpeeds;

            public readonly int Length;
        }

        [Inject] private BlackHoleScaleGroup m_BlackHoleScaleGroup;
#pragma warning restore 649

        [BurstCompile]
        struct BlackHoleScaleJob : IJobParallelFor
        {
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<Scale> localScales;
            public ComponentDataArray<BlackHoleScaleSpeed> scaleSpeeds;

            [ReadOnly]
            public float dt;

            public void Execute(int i)
            {
                // do stuff
                var newScaleVal = localScales[i].Value.x + (scaleSpeeds[i].Value * dt);
                newScaleVal = Mathf.Clamp(newScaleVal, 0, .1f); //clamp the scale so it grows to size 5.

                //optimize later by removing this component


                localScales[i] = new Scale
                {
                    Value = new float3(newScaleVal, newScaleVal, newScaleVal)
                };

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var blackHoleScaleJob = new BlackHoleScaleJob();
            blackHoleScaleJob.positions = m_BlackHoleScaleGroup.positions;
            blackHoleScaleJob.localScales = m_BlackHoleScaleGroup.localScales;
            blackHoleScaleJob.scaleSpeeds = m_BlackHoleScaleGroup.scaleSpeeds;

            //blackHoleChildPositionJob.moveSpeeds = m_BlackHoleChildGroup.moveSpeeds;
            blackHoleScaleJob.dt = Time.deltaTime;
            return blackHoleScaleJob.Schedule(m_BlackHoleScaleGroup.Length, 64, inputDeps);
        }

    }
}


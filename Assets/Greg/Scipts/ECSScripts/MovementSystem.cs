using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;



namespace Greg
{
    public class MovementSystem : JobComponentSystem
    {
        [ComputeJobOptimization]
        struct MovementJob : IJobProcessComponentData<Position, Rotation> //Position, Rotation, MoveSpeed filter
        {
            //public float stuff1;
            //public float stuff2;
            public float dt;

            public void Execute(ref Position position, [ReadOnly] ref Rotation rotation)
            {
                float3 value = position.Value;

                //value += dt * speed.speed * math.forward(rotation.Value);

                position.Value = value;

            }
        }
        /*
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            MovementJob moveJob = new MovementJob
            {
                //stuff1 = GameManager.instance.stuff1,
                //stuff2 = GameManager.instance.stuff2,
                dt = Time.deltaTime
            };

            JobHandle moveHandle = moveJob.Schedule(this, 64, inputDeps);

            return moveHandle;
        }
        */

    }
}
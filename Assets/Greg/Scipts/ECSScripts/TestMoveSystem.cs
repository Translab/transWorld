using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;



namespace Greg
{
    //public class InitWorldBarrier: BarrierSystem { }
    public class TestMovementSystem : JobComponentSystem
    {
        /*
        [ComputeJobOptimization]
        struct TestMovementJob: IJobProcessComponentData<Position, Rotation>
        {

            public float dt;


            public void Execute(ref Position position, ref Rotation rotation)
            {
                float3 value = position.Value;

                //value += dt * speed.speed * math.forward(rotation.Value);

                position.Value = value;
            }
        }

        //[Inject] private InitWorldBarrier m_Barrier;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Debug.Log("init world job called");
            //var sourceEntity = m_Group.Entity[0];

            TestMovementJob movementJob = new TestMovementJob
            {
                dt = Time.deltaTime
            };

            JobHandle moveHandle = movementJob.Schedule(this, 64, inputDeps);

            return moveHandle;
        }
        */
    }
}

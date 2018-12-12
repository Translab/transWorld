using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;



namespace Greg
{
    public class InitSystem : JobComponentSystem
    {
        [ComputeJobOptimization]
        struct InitJob : IJobProcessComponentData<Position, NumberOfSystems> //Position, Rotation, MoveSpeed filter
        {
            //public float stuff1;
            //public float stuff2;
            public int numSystems;
            public float dt;

            public void Execute(ref Position position, ref NumberOfSystems numSystems)
            {
                float3 value = position.Value;

                //value += dt * speed.speed * math.forward(rotation.Value);

                position.Value = value;

                //for each system
                //  instantiate an entity and tell it to init. need to keep track of level of detail. pass in level after subtracting. If == 0 then dont init
                //      
                //      TODO in MovementJob: need to be passing in movement speed and figure out how to move in circle around target location.
                //      what about parent transforms? With parent transforms I only need to set local position. 

            }
        }
        /*
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            InitJob initJob = new InitJob
            {
                //stuff1 = GameManager.instance.stuff1,
                //stuff2 = GameManager.instance.stuff2,

                dt = Time.deltaTime
                
            };

            //JobHandle moveHandle = initJob.Schedule(this, 64, inputDeps);

            return moveHandle;
        }
        */

    }
}

using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;



namespace Greg
{
    /*
    //public class InitWorldBarrier: BarrierSystem { }
    public class InitWorld2System : JobComponentSystem
    {
        [ComputeJobOptimization]
        struct InitWorldJob : IJobProcessComponentData<Position, SpawnNewSystem, SystemDepth> //Position, Rotation, MoveSpeed filter
        {
            //public float stuff1;
            //public float stuff2;
            //public float dt;

            public void Execute(ref Position position, [ReadOnly] ref SpawnNewSystem spawnSystem, ref SystemDepth depth)
            {
                EntityManager.Instantiate(spawnSystem.prefab, )

                //for each entity SetComponentData

                //float3 value = position.Value;

                //value += dt * speed.speed * math.forward(rotation.Value);

                //position.Value = value;

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            InitWorldJob worldJob = new InitWorldJob
            {
                //stuff1 = GameManager.instance.stuff1,
                //stuff2 = GameManager.instance.stuff2,
                //dt = Time.deltaTime




            };

            JobHandle worldHandle = worldJob.Schedule(this, 64, inputDeps);

            return worldHandle;
        }
    }
    */
}
        /*
        //data used in the job
        struct Group
        {
            //[ReadOnly] public int Length;
            [ReadOnly] public SharedComponentDataArray<SpawnNewSystem> Spawner;
            public ComponentDataArray<SystemDepth> Depth;
            public ComponentDataArray<Position> Positon;
            public EntityArray Entity;
            [ReadOnly] public int Length;

            //[ReadOnly] public EntityArray Entities;
            //[ReadOnly] public ComponentDataArray<InitWorld> InitWorlds;
        }
        [Inject] Group m_Group;

        //[Inject] private InitWorldBarrier m_Barrier;

        protected override void OnUpdate()
        {
            for (int j = 0; j < m_Group.Length; j++)
            {
                //Debug.Log("init world job called");
                var sourceEntity = m_Group.Entity[j]; //set a parent
                var spawner = m_Group.Spawner[j]; //grab stuff
                var center = m_Group.Positon[j].Value;
                var currDepth = m_Group.Depth[j].Value;

                var count = 10;

                Debug.Log(m_Group.Length);
                Debug.Log(currDepth);
                Debug.Log("break");
                var entities = new NativeArray<Entity>(count, Allocator.Temp);
                var positions = new NativeArray<float3>(count, Allocator.Temp);

                if (currDepth < spawner.maxDepth)
                {

                    EntityManager.Instantiate(spawner.prefab, entities); //instantiate another spawner

                    //gen a system of positions 

                    for (int i = 0; i < count; i++)
                    {
                        Debug.Log(i);
                        //new position info
                        positions[i] = new float3(center.x + Random.Range(0, 10), center.y + Random.Range(0, 10), center.z + Random.Range(0, 10));
                        var position = new LocalPosition
                        {
                            Value = positions[i]
                        };

                        //new spawn info
                        //var newSpawn = new SpawnNewSystem
                        //{
                        //    prefab = spawner.prefab,
                        //spawnLocal = true,
                        //radius = 10,
                        //    systemDepth = spawner.systemDepth + 1,

                        //    maxDepth = spawner.maxDepth,
                        //    isSpawning = false
                        //};

                        var systemDepth = new SystemDepth
                        {
                            Value = currDepth + 1
                        };

                        EntityManager.SetComponentData(entities[i], new TransformParent { Value = sourceEntity });
                        EntityManager.SetComponentData(entities[i], position);
                        EntityManager.SetComponentData(entities[i], systemDepth);

                        //EntityManager.SetSharedComponentData(entities[i], newSpawn);
                        //set the component

                        //EntityManager.SetComponentData(entities[i], new SpawnNewSystem { systemDepth = spawner.systemDepth + 1 });

                        // Spawn Attach - I think this is the part for the parenting
                        //var attach = EntityManager.CreateEntity();
                        //EntityManager.AddComponentData(attach, new Attach
                        //{
                        //    Parent = sourceEntity,
                        //    Child = entities[i]
                        //});
                    }
                }
                Debug.Log("clean up");
                entities.Dispose();
                positions.Dispose();
                //memory clean up

                EntityManager.RemoveComponent<SpawnNewSystem>(sourceEntity);

            }
            Debug.Log("finish update");
            UpdateInjectedComponentGroups();

        }

        /*
        [ComputeJobOptimization]
        struct InitWorldJob : IJobProcessComponentData<Position, Rotation, InitWorld> //Position, Rotation, InitWorld filter
        {
            //public float stuff1;
            //public float stuff2;
            public float dt;

            public void Execute(ref Position position, ref Rotation rotation, ref InitWorld initWorld)
            {
                //float3 value = position.Value;
                Debug.Log("in InitWorld execute");

                //value += dt * speed.speed * math.forward(rotation.Value);

                //position.Value = value;

            }
        }

        [ComputeJobOptimization]
        struct RemoveComponentJob: IJobParallelFor
        {
            [WriteOnly] public EntityCommandBuffer Cmd;
            [ReadOnly] public EntityArray Entities;

            public void Execute(int index)
            {
                Cmd.RemoveComponent<InitWorld>(Entities[index]);
            }
        }

        /*
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            InitWorldJob initWorldJob = new InitWorldJob()
            {
                //topBound = GameManager.GM.topBound,
                //bottomBound = GameManager.GM.bottomBound,
                dt = Time.deltaTime
            }; 
            //make a new job

            //schedule the job
            JobHandle initHandle = initWorldJob.Schedule(this, 64, inputDeps);
            initHandle.Complete(); //wait for job to complete

            RemoveComponentJob removeJob = new RemoveComponentJob()
            {
                Cmd = m_Barrier.CreateCommandBuffer(),
                Entities = m_Data.Entities
            }
            
            JobHandle removeJobHandle = removeJob.Schedule(m_Data, 1, inputDeps);
            return removeJobHandle;


            //return inputDeps;
        }
        */


        /*
        protected override void OnCreateManager(int capacity) {
            InitJob initJob = new InitJob
            {
                dt = Time.deltaTime
            };


            JobHandle initHandle = initJob.Schedule(this, 64, inputDeps);

            //return initHandle;

        }
        */

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


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
    public class RingRevolveJobSystem : JobComponentSystem
    {
#pragma warning disable 649
        struct RingRevolveGroup
        {

            public ComponentDataArray<Position> positions;

            public ComponentDataArray<RingRevolve> ringRevolve;
            [ReadOnly]
            public ComponentDataArray<MoveSpeed> moveSpeeds;

            [ReadOnly] public ComponentDataArray<Depth> depths;

            public readonly int Length;
        }

        [Inject] private RingRevolveGroup m_RingRevolveGroup;
#pragma warning restore 649

        [BurstCompile]
        struct RingRevolvePosition : IJobParallelFor
        {
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<RingRevolve> ringRevolve;
            [ReadOnly]
            public ComponentDataArray<MoveSpeed> moveSpeeds;
            [ReadOnly]
            public float dt;
            [ReadOnly]
            public ComponentDataArray<Depth> depths;
            public float d1BaseSpeedMultiplyer;
            public float d2BaseSpeedMultiplyer;
            public float d3BaseSpeedMultiplyer;
            public float d1Harmonicity;
            public float d2Harmonicity;
            public float d3Harmonicity;
            public float d1Radius;
            public float d2Radius;
            public float d3Radius;

            //depending on level I can filter
            //but for each level how to set the parameters
            //speed goes to 0 to whatever
            //basically select from an array of posible speed {1,2,3,4,5} but stay same sometimes
            //what about z rotation.
            //but if I keep resetting them then it will look like nothing....
            //need an IProcessComponentData
            //need a separate job to control the movespeed and z rotation.
            //each object gets an init speed at a harmonic ratio so drawn from a hat.
            //each objects gets an init rand speed for whole range of speeds available
            //each object gets an init z rot at a harmonic ratio so drawn from hat
            //each object gets a random z rot
            //distance affects speed of d1 components.
            //random walk of frequency range base speed multiplayer for d2 d3 components - TODO IN THIS METHOD
            //what about harmonicity of d1 components?
            //
            //You then introduce random perturbation
            //how?
            //add a random value to the speed
            //no way to keep track of which entities have 

            public void Execute(int i)
            {
                float baseSpeedMultiplyer;
                float harmonicity;
                float radius;

                if (depths[i].depth == 1)
                {
                    baseSpeedMultiplyer = d1BaseSpeedMultiplyer;
                    harmonicity = d1Harmonicity;
                    radius = ringRevolve[i].radius * d1Radius;
                }
                else if (depths[i].depth == 2)
                {
                    baseSpeedMultiplyer = d2BaseSpeedMultiplyer;
                    harmonicity = d2Harmonicity;
                    radius = ringRevolve[i].radius * d2Radius;
                }
                else
                {
                    baseSpeedMultiplyer = d3BaseSpeedMultiplyer;
                    harmonicity = d3Harmonicity;
                    radius = ringRevolve[i].radius * d3Radius;
                }

                float newMoveSpeed = (math.lerp(ringRevolve[i].initRandSpeed, ringRevolve[i].initSpeed, harmonicity)) * baseSpeedMultiplyer;
                float newZRotSpeed = (math.lerp(ringRevolve[i].initRandZRotSpeed, ringRevolve[i].initZRotSpeed, harmonicity)) * baseSpeedMultiplyer;


                //ring rotation 
                //float zRotation = (ringRevolve[i].zRotation + (dt * ringRevolve[i].initZRotSpeed)) % 360; //modulo from 0 360
                float zRotation = (ringRevolve[i].zRotation + (dt * newZRotSpeed)) % 360; //modulo from 0 360


                //float t = ringRevolve[i].t + (dt * moveSpeeds[i].speed);
                float t = ringRevolve[i].t + (dt * newMoveSpeed);

                float offsetT = t + (0.01f * i) + ringRevolve[i].initOffset;
                float cosVal = math.cos(offsetT);
                float sinVal = math.sin(offsetT);

                //TODO : CHANGE THE RADIUS
                float x = cosVal * radius;
                float y = 0;
                float z = sinVal * radius;


                //float x = ringRevolve[i].center.x + (cosVal * ringRevolve[i].radius);
                //float y = ringRevolve[i].center.y;
                //float z = ringRevolve[i].center.z + (sinVal * ringRevolve[i].radius);


                //math.sin(offsetT) //get 

                //points[i] = ringRotation * points[i];
                //float zRot = ringRevolve[] + math.sin(offsetT);

                //convert to 360


                Quaternion rotation = Quaternion.Euler(0f, 0f, zRotation);
                


                ringRevolve[i] = new RingRevolve
                {
                    t = t,
                    //center = ringRevolve[i].center,
                    radius = ringRevolve[i].radius,
                    initOffset = ringRevolve[i].initOffset,
                    zRotation = zRotation,

                    initSpeed = ringRevolve[i].initSpeed,
                    initRandSpeed = ringRevolve[i].initRandSpeed,
                    initZRotSpeed = ringRevolve[i].initZRotSpeed,
                    initRandZRotSpeed = ringRevolve[i].initRandZRotSpeed,

                    //zSpeed = ringRevolve[i].zSpeed
                };

                //float3 newPosition = ringRevolve[i].zRotation * new Vector3(x, y, z);

                positions[i] = new Position
                {
                    Value = rotation * new float3(x, y, z)
                };
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var ringRevolvePositionJob = new RingRevolvePosition();
            ringRevolvePositionJob.positions = m_RingRevolveGroup.positions;
            ringRevolvePositionJob.ringRevolve = m_RingRevolveGroup.ringRevolve;
            ringRevolvePositionJob.moveSpeeds = m_RingRevolveGroup.moveSpeeds;
            ringRevolvePositionJob.depths = m_RingRevolveGroup.depths;

            Debug.Log(m_RingRevolveGroup.ringRevolve[0].radius);

            //parameters
            ringRevolvePositionJob.d1BaseSpeedMultiplyer = ParameterManagerGreg.instance.D1BaseSpeedMultiplyer();
            ringRevolvePositionJob.d2BaseSpeedMultiplyer = ParameterManagerGreg.instance.D2BaseSpeedMultiplyer();
            ringRevolvePositionJob.d3BaseSpeedMultiplyer = ParameterManagerGreg.instance.D3BaseSpeedMultiplyer();
            ringRevolvePositionJob.d1Harmonicity = ParameterManagerGreg.instance.D1Harmonicity();
            ringRevolvePositionJob.d2Harmonicity = ParameterManagerGreg.instance.D2Harmonicity();
            ringRevolvePositionJob.d3Harmonicity = ParameterManagerGreg.instance.D3Harmonicity();
            ringRevolvePositionJob.d1Radius = ParameterManagerGreg.instance.getD1Radius();
            Debug.Log(ParameterManagerGreg.instance.getD1Radius());

            ringRevolvePositionJob.d2Radius = ParameterManagerGreg.instance.getD2Radius();
            ringRevolvePositionJob.d3Radius = ParameterManagerGreg.instance.getD3Radius();

            // what about scale and direction of black hole particles

            ringRevolvePositionJob.dt = Time.deltaTime;
            return ringRevolvePositionJob.Schedule(m_RingRevolveGroup.Length, 64, inputDeps);
        }
        
    }
}


using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using System;


[ComputeJobOptimization]
public struct ElementMovementJob : IJobParallelForTransform
{
    public float moveSpeed;
    public float deltaTime; //no concept of delta time in job system

    public void Execute(int index, TransformAccess transform)
    {
        Vector3 pos = transform.position;
        pos += moveSpeed * deltaTime * (transform.rotation * new Vector3(0f, 0f, 1f));
    }

}


internal class ComputeJobOptimizationAttribute : Attribute
{

}


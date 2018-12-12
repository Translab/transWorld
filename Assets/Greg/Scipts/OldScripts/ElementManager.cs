using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ElementManager : MonoBehaviour {

    TransformAccessArray transforms;
    ElementMovementJob moveJob;
    JobHandle moveHandle;

    private void OnDisable()
    {
        moveHandle.Complete();
        transforms.Dispose(); //clear up memory when done using them
    }

    // Use this for initialization
    void Start () {
        transforms = new TransformAccessArray(0, -1);
	}
	
	// Update is called once per frame
	void Update () {
        moveHandle.Complete();

        moveJob = new ElementMovementJob()
        {
            moveSpeed = 2,
            deltaTime = Time.deltaTime
        };

        moveHandle = moveJob.Schedule(transforms);

        JobHandle.ScheduleBatchedJobs();
	}
}

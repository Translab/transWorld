using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GenSpawns : MonoBehaviour {

    public GameObject spawnPrefab;
    private NativeArray<Entity> spawnEntity; //holds all spawn points
    EntityManager em;


    // Use this for initialization
    void Start () {
        Debug.Log("entity spawn start");

        //grab the entity manager
        em = World.Active.GetOrCreateManager<EntityManager>();

        spawnEntity = new NativeArray<Entity>(1, Allocator.Temp); //world entity
        spawnEntity[0] = em.Instantiate(spawnPrefab); //instantiate world entity

        em.SetComponentData(spawnEntity[0], new Position { Value = new float3(2, 2, 2) });

        //spawnEntity[0]
        Debug.Log("spawn end");

        spawnEntity.Dispose();
    }

    //dispose the entity when the game is disabled (so no memory leaks)
    void OnDisable()
    {
        //spawnEntity.Dispose();
    }

    // Update is called once per frame
    void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Entities;
using UnityEngine.Rendering;

public class Bootstrap : MonoBehaviour {

    public Mesh mesh;
    public Material mat;
    /*
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void AfterScene()
    {
        Debug.Log("after scene load");

        //spawn cubes with random headings

        Bootstrap bootstrap = GameObject.Find("Bootstrap").GetComponent<Bootstrap>();
        MeshInstanceRenderer cubeRenderer = new MeshInstanceRenderer
        {
            mesh = bootstrap.mesh,
            material = bootstrap.mat,
            subMesh = 0,            //int (index of a submesh within the mesh)
            castShadows = ShadowCastingMode.Off,
            receiveShadows = false
        };

        EntityManager em = World.Active.GetOrCreateManager<EntityManager>();
        EntityArchetype cubeArchetype = em.CreateArchetype(
            typeof(Position),
            typeof(Heading),
            typeof(TransformMatrix),
            typeof(MoveSpeed),
            typeof(MoveForward)
        );

        for (int i =0; i< 30; i++)
        {
            var cubeEntity = em.CreateEntity(cubeArchetype);
            em.SetComponentData(cubeEntity, new Position { Value = new float3(0, 0, 0) });
            em.SetComponentData(cubeEntity, new Heading { Value = Random.onUnitSphere }); //Vector3 coerced to float3
            em.SetComponentData(cubeEntity, new MoveSpeed { speed = 1 });
            em.AddSharedComponentData(cubeEntity, cubeRenderer);
        }
    }
    */
}

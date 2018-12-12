using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    //variables used to initialize the world and spawn entities of depth 1.
    [Serializable]
    public struct Spawn : ISharedComponentData
    {
        public GameObject prefab;
        public float radius;
        public int ringsPerSystem;
        public int objsPerRing;
        public int count;
    }

    public class SpawnComponent : SharedComponentDataWrapper<Spawn> { }
}

using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Greg
{
    /// <summary>
    /// This component will update the corresponding PositionComponent associated with this component at the
    /// rate specified by the MoveSpeedComponent, also associated with this component in radians per second.
    /// </summary>
    [Serializable]
    public struct BlackHoleSpawn : IComponentData
    {
        public float3 center; //may be able to remove this
        public float radius; //radius of the black hole system
        public GameObject prefab;
        //public int count;
        public int maxCount;

        [NonSerialized]
        public float t;

    }

    public class BlackHoleSpawnComponent : ComponentDataWrapper<BlackHoleSpawn> { }
}


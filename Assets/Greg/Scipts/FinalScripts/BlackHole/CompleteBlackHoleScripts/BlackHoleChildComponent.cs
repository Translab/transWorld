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
    public struct BlackHoleChild : IComponentData
    {
        public Entity parent; //store ref to parent
        public float3 acceleration; //add acceleration to child
        public float3 velocity; //add velocity to child
        public float destroyRadius; //add a radius at which the entity will be scheduled for resetting
        public float spawnRadius; //add a spawn radius
        [NonSerialized]
        public float t;
    }

    public class BlackHoleChildComponent : ComponentDataWrapper<BlackHoleChild> { }
}

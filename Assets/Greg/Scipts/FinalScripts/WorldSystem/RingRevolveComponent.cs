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
    public struct RingRevolve : IComponentData
    {
        //public float3 center; //may be able to remove this
        public float initOffset;
        public float zRotation; //changed to float
        //public float zSpeed; // z rotation speed
        public float initSpeed; // initial cube speed
        public float initRandSpeed; 
        public float initZRotSpeed;
        public float initRandZRotSpeed;
        public float radius;
        [NonSerialized]
        public float t;
    }

    [UnityEngine.DisallowMultipleComponent]
    public class RingRevolveComponent : ComponentDataWrapper<RingRevolve> { }
}
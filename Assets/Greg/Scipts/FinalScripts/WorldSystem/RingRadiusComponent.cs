using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    //variables used to initialize the world and spawn entities of depth 1.
    [Serializable]
    public struct RingRadius : IComponentData
    {
        public float Value;
    }

    public class RingRadiusComponent : ComponentDataWrapper<RingRadius> { }
}

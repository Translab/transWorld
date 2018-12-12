using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct Depth : IComponentData
    {
        public int depth;
    }

    public class DepthComponent : ComponentDataWrapper<Depth> { }
}

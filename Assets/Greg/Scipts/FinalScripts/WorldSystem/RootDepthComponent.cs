using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct RootDepth : IComponentData
    {
    }

    public class RootDepthComponent : ComponentDataWrapper<RootDepth> { }
}
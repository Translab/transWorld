using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct Depth1 : IComponentData
    {
    }

    public class Depth1Component : ComponentDataWrapper<Depth1> { }
}

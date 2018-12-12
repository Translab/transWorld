using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct Depth2 : IComponentData
    {
    }

    public class Depth2Component : ComponentDataWrapper<Depth2> { }
}

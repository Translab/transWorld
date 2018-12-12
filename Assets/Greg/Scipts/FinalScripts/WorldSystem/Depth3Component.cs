using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct Depth3 : IComponentData
    {
    }

    public class Depth3Component : ComponentDataWrapper<Depth3> { }
}

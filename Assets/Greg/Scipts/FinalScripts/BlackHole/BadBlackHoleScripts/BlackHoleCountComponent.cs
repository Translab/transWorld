using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct BlackHoleCount : IComponentData
    {
        public int count;
    }

    public class BlackHoleCountComponent : ComponentDataWrapper<BlackHoleCount> { }
}

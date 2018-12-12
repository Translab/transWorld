using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct BlackHoleScaleSpeed : IComponentData
    {
        public float Value;
    }

    public class BlackHoleScaleSpeedComponent : ComponentDataWrapper<BlackHoleScaleSpeed> { }
}


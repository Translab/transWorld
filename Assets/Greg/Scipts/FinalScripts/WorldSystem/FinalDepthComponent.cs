using System;
using Unity.Entities;
using UnityEngine;

namespace Greg
{
    [Serializable]
    public struct FinalDepth : IComponentData
    {
    }

    public class FinalDepthComponent : ComponentDataWrapper<FinalDepth> { }
}

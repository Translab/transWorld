using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace Greg
{
    [Serializable]
    public struct InitBlackHoleSpawn : IComponentData
    {
    }

    public class InitBlackHoleSpawnComponent : ComponentDataWrapper<InitBlackHoleSpawn> { }
}

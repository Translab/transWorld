using System;
using Unity.Entities;

namespace Unity.Transforms
{
    //Stores number of child systems for a given parent system
    [Serializable]
    public struct NumberOfSystems : IComponentData
    {
        public int Value;

        public NumberOfSystems(int numSystems)
        {
            Value = numSystems;
        }
    }

    public class NumberOfSystemsComponent : ComponentDataWrapper<NumberOfSystems> { }
}
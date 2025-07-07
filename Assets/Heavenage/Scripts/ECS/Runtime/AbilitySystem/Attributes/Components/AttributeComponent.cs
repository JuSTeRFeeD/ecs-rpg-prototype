using System;
using System.Collections.Generic;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components
{
    [Serializable]
    public struct AttributeEntry
    {
        public float baseValue;
        public float bonusValue;
        public float multiplier;
        
        public float FinalValue => baseValue + bonusValue * (1f + multiplier);

        public float currentValue;
    }
    
    [Serializable]
    public struct AttributeComponent : IComponent
    {
        public Dictionary<int, AttributeEntry> AttributeMap;
    }
}
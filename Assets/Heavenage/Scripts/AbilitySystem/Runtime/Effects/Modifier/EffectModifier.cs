using Heavenage.Scripts.AbilitySystem.Runtime.Attributes;
using Heavenage.Scripts.AbilitySystem.Runtime.Attributes.Modifiers;
using Heavenage.Scripts.AbilitySystem.Runtime.Components;
using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Effects.Modifier
{
    [System.Serializable]
    public struct EffectModifier
    {
        [SerializeField] private AttributeDefinition targetAttribute;
        [SerializeField] private ModifierOperation operation;
        [SerializeField] private float magnitude;

        public AttributeDefinition TargetAttribute => targetAttribute;
        public ModifierOperation Operation => operation;
        public float Magnitude => magnitude;
        
        public void Apply(AbilitySystemComponent targetAsc)
        {
            var attribute = targetAsc.AttributeSet.GetAttributeByDefinition(targetAttribute);
            attribute.AddModifier(new AttributeModifierHandle(new AttributeModifier(magnitude, operation), 0));
        }
    }
}
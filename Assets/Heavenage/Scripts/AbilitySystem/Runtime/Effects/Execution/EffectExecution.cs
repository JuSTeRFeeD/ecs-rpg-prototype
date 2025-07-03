using Heavenage.Scripts.AbilitySystem.Runtime.Attributes;
using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Effects.Execution
{
    public class EffectExecution : ScriptableObject
    {
        public virtual void OnEffectApplied(GameplayEffectInstance instance) { }
        public virtual void OnEffectRemoved(GameplayEffectInstance instance) { }
        
        public virtual void OnTick(GameplayEffectInstance instance, float deltaTime) { }

        public virtual void OnAttributeChanged(GameplayEffectInstance instance, AttributeDefinition attribute, float prevValue, float newValue) { }
    }
}
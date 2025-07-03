using Heavenage.Scripts.AbilitySystem.Runtime.Effects.Execution;
using Heavenage.Scripts.AbilitySystem.Runtime.Effects.Modifier;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Effects
{
    public enum EffectsDurationPolicy
    {
        Instant = 0,
        Infinite = 1,
        Duration = 2
    }
    
    [CreateAssetMenu(menuName = "Ability System/Gameplay Effect")]
    public class GameplayEffect : ScriptableObject
    {
        [SerializeField] private EffectsDurationPolicy durationPolicy;
        [MinValue(0f)]
        [SerializeField] private float duration = 0f;
        [MinValue(0f)]
        [SerializeField] private float period = 0f;
        [MinValue(0)]
        [SerializeField] private int maxTicks = 0;
        [Space]
        [SerializeField] private EffectModifier[] modifiers;
        [SerializeField] private EffectExecution[] executions;
        
        public EffectsDurationPolicy DurationPolicy => durationPolicy;
        public float Duration => duration;
        public float Period => period;
        public int MaxTicks => maxTicks;
        
        public EffectModifier[] Modifiers => modifiers;
        public EffectExecution[] Executions => executions;
    }
}
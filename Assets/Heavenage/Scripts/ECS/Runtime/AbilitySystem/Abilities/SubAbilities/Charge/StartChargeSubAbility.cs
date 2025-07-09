using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge.TargetingModifiers;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Heavenage.Scripts.ECS.Runtime.Views;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge
{
    public struct ChargingAbilityComponent : IComponent
    {
        public float ElapsedTime;
        public float MaxChargeTime;
        public bool AutoReleaseOnMaxCharge;
    }
    
    [CreateAssetMenu(menuName = "RPG/SubAbility/StartCharge")]
    public class StartChargeSubAbility : SubAbilityTask<StartChargeAbilityTask>
    {
        [Tooltip("Should auto release on max charge")]
        public bool releaseOnMaxCharge;
        [MinValue(0.1f)] public float maxChargeTime = 1f;
        [Space]
        [Required, InlineEditor] public TargetingStrategy targetingStrategy;
        [Required] public EntityView indicatorViewPrefab;
        [Space]
        [Optional, InlineEditor] public TargetingModifier targetingModifier;
    }
    
    public class StartChargeAbilityTask : IAbilityTaskWithSetup
    {
        private float _maxChargeTime;
        private bool _autoReleaseOnMaxCharge;
        private ITargetingStrategy _targetingStrategy;
        private ITargetingModifier _targetingModifier;
        
        public void SetupFromSubAbility(SubAbility ability)
        {
            var data = (StartChargeSubAbility)ability;
            _maxChargeTime = data.maxChargeTime;
            _autoReleaseOnMaxCharge = data.releaseOnMaxCharge;

            _targetingStrategy = data.targetingStrategy.CreateTargetingStrategy(data.indicatorViewPrefab);
            _targetingModifier = data.targetingModifier;
        }

        public bool Tick(Entity activeAbility, Entity caster, Entity target, World world, float deltaTime)
        {
            // Methods to use:
            _targetingStrategy.OnStart(activeAbility, world);

            var chargingStash = StashRegistry.GetStash<ChargingAbilityComponent>();
            var inputReleasedStash = StashRegistry.GetStash<InputReleasedTag>();
            
            if (!chargingStash.Has(activeAbility))
            {
                chargingStash.Add(activeAbility, new ChargingAbilityComponent
                {
                    ElapsedTime = 0f,
                    MaxChargeTime = _maxChargeTime,
                    AutoReleaseOnMaxCharge = _autoReleaseOnMaxCharge
                });
            }
            
            ref var charging = ref chargingStash.Get(activeAbility);
            charging.ElapsedTime += deltaTime;
            
            _targetingModifier?.Modify(activeAbility, world, _targetingStrategy, charging.ElapsedTime / charging.MaxChargeTime);
            
            _targetingStrategy.Tick(activeAbility, caster, world);

            if (charging.AutoReleaseOnMaxCharge && charging.ElapsedTime >= charging.MaxChargeTime || 
                inputReleasedStash.Has(activeAbility)) 
            {
                var resolvedTargets = _targetingStrategy.GetTargets(activeAbility, world);
                var resolvedTargetsStash = StashRegistry.GetStash<ResolvedTargetsComponent>();
                ref var targets = ref resolvedTargetsStash.GetOrAdd(activeAbility);
                targets.Targets ??= new List<Entity>(resolvedTargets.Count);
                targets.Targets.AddRange(resolvedTargets);
                
                _targetingStrategy.OnEnd(activeAbility, world);
                return true;
            }
            
            return false;
        }
    }
}
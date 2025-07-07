using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge.TargetingModifiers;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting;
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

        public bool Tick(Entity caster, Entity target, World world, float deltaTime)
        {
            // Methods to use:
            _targetingStrategy.OnStart(caster, world);

            var chargingStash = StashRegistry.GetStash<ChargingAbilityComponent>();
            if (!chargingStash.Has(caster))
            {
                chargingStash.Add(caster, new ChargingAbilityComponent
                {
                    ElapsedTime = 0f,
                    MaxChargeTime = _maxChargeTime,
                    AutoReleaseOnMaxCharge = _autoReleaseOnMaxCharge
                });
            }
            
            ref var charging = ref chargingStash.Get(caster);
            chargingStash.Get(caster).ElapsedTime += deltaTime;
            
            _targetingModifier?.Modify(caster, world, _targetingStrategy, charging.ElapsedTime / charging.MaxChargeTime);
            
            _targetingStrategy.Tick(caster, world);

            // todo: resolve on input key release
            if (charging.AutoReleaseOnMaxCharge && charging.ElapsedTime >= charging.MaxChargeTime) 
            {
                // TODO: use resolved targets
                var resolvedTargets = _targetingStrategy.GetTargets(caster, world);
                _targetingStrategy.OnEnd(caster, world);

                // TODO: clean up charging
                chargingStash.Remove(caster);
                
                return true;
            }
            
            return false;
        }
    }
}
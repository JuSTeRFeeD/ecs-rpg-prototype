using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Damage
{
    [CreateAssetMenu(menuName = "RPG/SubAbility/Damage/ApplyDoT")]
    public class ApplyDoTSubAbility : SubAbilityTask<ApplyDoTAbilityTask>
    {
        public DotStackPolicy stackPolicy;
        public DamageType damageType;
        public float totalDuration = 3f;
        public float tickInterval = 1f;
        public float damagePerTick;
    }

    public class ApplyDoTAbilityTask : IAbilityTaskWithSetup
    {
        private DotStackPolicy _stackPolicy;
        private DamageType _damageType;
        private float _totalDuration;
        private float _tickInterval;
        private float _damagePerTick;

        public void SetupFromSubAbility(SubAbility ability)
        {
            var data = (ApplyDoTSubAbility)ability;
            _stackPolicy = data.stackPolicy;
            _damageType = data.damageType;
            _totalDuration = data.totalDuration;
            _tickInterval = data.tickInterval;
            _damagePerTick = data.damagePerTick;
        }

        public bool Tick(Entity activeAbility, Entity caster, Entity target, World world, float deltaTime)
        {
            var dotStash = StashRegistry.GetStash<DotEffectComponent>();
            var existingDots = world.Filter.With<DotEffectComponent>().Build();

            var dotApplied = false;

            foreach (var dotEntity in existingDots)
            {
                ref var existing = ref dotStash.Get(dotEntity);

                if (existing.Target == target && existing.DamageType == _damageType && existing.Source == caster)
                {
                    switch (existing.StackPolicy)
                    {
                        case DotStackPolicy.Overwrite:
                            existing.Duration = _totalDuration;
                            existing.TickInterval = _tickInterval;
                            existing.DamagePerTick = _damagePerTick;
                            existing.Elapsed = 0f;
                            dotApplied = true;
                            break;

                        case DotStackPolicy.Refresh:
                            existing.Duration = _totalDuration;
                            dotApplied = true;
                            break;

                        case DotStackPolicy.MaxDuration:
                            existing.Duration = Mathf.Max(existing.Duration, _totalDuration);
                            dotApplied = true;
                            break;

                        case DotStackPolicy.Stack:
                            // do nothing — мы создадим новый
                            break;
                    }

                    if (dotApplied) break;
                }
            }

            if (!dotApplied || _stackPolicy == DotStackPolicy.Stack)
            {
                var dotEntity = world.CreateEntity();
                dotStash.Set(dotEntity, new DotEffectComponent
                {
                    Target = target,
                    Source = caster,
                    DamageType = _damageType,
                    Duration = _totalDuration,
                    TickInterval = _tickInterval,
                    DamagePerTick = _damagePerTick,
                    StackPolicy = _stackPolicy
                });
            }

            return true;
        }
    }
}
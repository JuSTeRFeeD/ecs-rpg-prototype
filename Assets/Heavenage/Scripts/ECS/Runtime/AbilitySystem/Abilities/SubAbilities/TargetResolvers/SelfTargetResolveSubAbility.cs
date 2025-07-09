using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using NUnit.Framework;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.TargetResolvers
{
    public class SelfTargetResolveSubAbility : SubAbility
    {
        public override IAbilityTask CreateTask()
        {
            return new SelfTargetResolveTask();
        }

        private class SelfTargetResolveTask : IAbilityTask
        {
            public bool Tick(Entity activeAbility, Entity caster, Entity target, World world, float deltaTime)
            {
                var resolvedTargetsStash = StashRegistry.GetStash<ResolvedTargetsComponent>();
                ref var targets = ref resolvedTargetsStash.GetOrAdd(activeAbility);
                targets.Targets ??= new List<Entity>(1);
                targets.Targets.Add(caster);
                return true;
            }
        }
    }
}
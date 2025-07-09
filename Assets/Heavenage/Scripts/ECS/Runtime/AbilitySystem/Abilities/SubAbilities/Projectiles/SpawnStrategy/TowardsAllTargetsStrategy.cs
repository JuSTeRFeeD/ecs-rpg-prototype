using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.SpawnStrategy
{
    [CreateAssetMenu(menuName = "RPG/ProjectileSpawnStrategy/TowardsAllTargets")]
    public class TowardsAllTargetsStrategy : ProjectileSpawnStrategy
    {
        public override IEnumerable<(Vector3, Entity?)> Resolve(Entity activeAbility, Entity caster, World world)
        {
            var resolvedTargetsStash = StashRegistry.GetStash<ResolvedTargetsComponent>();
            if (!resolvedTargetsStash.Has(activeAbility))
            {
                Debug.Log("Active ability has no ResolvedTargetsComponent");
                yield break;
            }

            var casterView = StashRegistry.GetStash<EntityViewComponent>().Get(caster).Value;
            var origin = casterView.transform.position;

            foreach (var target in resolvedTargetsStash.Get(activeAbility).Targets)
            {
                if (target.Equals(caster)) continue;
                
                var targetView = StashRegistry.GetStash<EntityViewComponent>().Get(target).Value;
                var dir = (targetView.transform.position - origin).normalized;
                yield return (dir, target);
            }
        }
    }
}
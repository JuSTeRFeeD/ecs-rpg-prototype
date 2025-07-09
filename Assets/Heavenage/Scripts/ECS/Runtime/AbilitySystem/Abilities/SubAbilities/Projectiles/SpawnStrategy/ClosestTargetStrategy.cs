using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.SpawnStrategy
{
    [CreateAssetMenu(menuName = "RPG/ProjectileSpawnStrategy/ClosestTarget")]
    public class ClosestTargetStrategy : ProjectileSpawnStrategy
    {
        public override IEnumerable<(Vector3, Entity?)> Resolve(Entity activeAbility, Entity caster, World world)
        {
            var resolvedTargetsStash = StashRegistry.GetStash<ResolvedTargetsComponent>();
            if (!resolvedTargetsStash.Has(activeAbility)) yield break;

            var viewStash = StashRegistry.GetStash<EntityViewComponent>();
            
            var casterView = viewStash.Get(caster).Value;
            var origin = casterView.transform.position;

            Entity? closest = null;
            var closestDistSqr = float.MaxValue;

            foreach (var target in resolvedTargetsStash.Get(activeAbility).Targets)
            {
                if (target == caster) continue;

                var targetView = viewStash.Get(target).Value;
                float distSqr = (targetView.transform.position - origin).sqrMagnitude;
                if (distSqr < closestDistSqr)
                {
                    closest = target;
                    closestDistSqr = distSqr;
                }
            }

            if (closest.HasValue)
            {
                var targetView = viewStash.Get(closest.Value).Value;
                var dir = (targetView.transform.position - origin).normalized;
                yield return (dir, closest);
            }
        }
    }

}
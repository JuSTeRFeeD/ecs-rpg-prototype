using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.SpawnStrategy
{
    [CreateAssetMenu(menuName = "RPG/ProjectileSpawnStrategy/CircleAround")]
    public class CircleAroundStrategy : ProjectileSpawnStrategy
    {
        [Range(1, 20)] public int projectileCount = 8;

        public override IEnumerable<(Vector3, Entity?)> Resolve(Entity activeAbility, Entity caster, World world)
        {
            for (var i = 0; i < projectileCount; i++)
            {
                var angle = 360f / projectileCount * i;
                var dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                yield return (dir.normalized, null);
            }
        }
    }
}

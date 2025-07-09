using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.SpawnStrategy
{
    [CreateAssetMenu(menuName = "RPG/ProjectileSpawnStrategy/ConeForward")]
    public class ConeForwardStrategy : ProjectileSpawnStrategy
    {
        [Range(1, 20)] public int projectileCount = 5;
        [Range(0, 180)] public float angle = 60f;

        public override IEnumerable<(Vector3, Entity?)> Resolve(Entity activeAbility, Entity caster, World world)
        {
            var view = StashRegistry.GetStash<EntityViewComponent>().Get(caster).Value;
            var forward = view.transform.forward;

            var halfAngle = angle * 0.5f;
            for (var i = 0; i < projectileCount; i++)
            {
                var lerp = projectileCount == 1 ? 0.5f : (float)i / (projectileCount - 1);
                var angleOffset = Mathf.Lerp(-halfAngle, halfAngle, lerp);
                var direction = Quaternion.Euler(0, angleOffset, 0) * forward;
                yield return (direction.normalized, null);
            }
        }
    }

}
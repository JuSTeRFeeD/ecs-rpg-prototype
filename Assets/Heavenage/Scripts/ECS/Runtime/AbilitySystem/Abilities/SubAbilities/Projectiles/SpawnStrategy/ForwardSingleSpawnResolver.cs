using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.SpawnStrategy
{
    [CreateAssetMenu(menuName = "RPG/ProjectileSpawnStrategy/ForwardSingleSpawn")]
    public abstract class ForwardSingleSpawnResolver : ProjectileSpawnStrategy
    {
        public override IEnumerable<(Vector3 direction, Entity? entity)> Resolve(Entity activeAbility, Entity caster, World world)
        {
            var view = StashRegistry.GetStash<EntityViewComponent>().GetOrAdd(caster).Value;
            Debug.Log("Resolved forward");
            yield return (view.transform.forward, null);
        }
    }
}
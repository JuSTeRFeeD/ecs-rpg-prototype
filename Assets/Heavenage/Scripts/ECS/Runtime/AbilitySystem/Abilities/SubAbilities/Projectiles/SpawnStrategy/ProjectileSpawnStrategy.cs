using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.SpawnStrategy
{
    public abstract class ProjectileSpawnStrategy : ScriptableObject, IProjectileSpawnStrategy
    {
        public abstract IEnumerable<(Vector3 direction, Entity? entity)> Resolve(Entity activeAbility, Entity caster, World world);
    }
}
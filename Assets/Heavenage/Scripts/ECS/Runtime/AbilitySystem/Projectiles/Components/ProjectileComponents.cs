using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.MovementLogic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Projectiles.Components
{
    public struct ProjectileComponent : IComponent
    {
        public Vector3 Direction;
        public float Speed;
        public Entity Source;
        public AbilityDefinition OnHitAbility;
        public bool DestroyOnHit;
    }

    public struct ActiveProjectileComponent : IComponent
    {
        public Entity Projectile;
        public IProjectileMovementLogic Logic;
    }

    public struct ProjectileHitComponent : IComponent
    {
        public Entity HitEntity;
    }
}
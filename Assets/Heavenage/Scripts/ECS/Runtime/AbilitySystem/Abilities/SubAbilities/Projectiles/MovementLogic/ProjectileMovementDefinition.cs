using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.MovementLogic
{
    public abstract class ProjectileMovementDefinition : ScriptableObject
    {
        public abstract IProjectileMovementLogic CreateLogic();
    }
}
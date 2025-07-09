using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.MovementLogic
{
    public interface IProjectileMovementLogic
    {
        void Initialize(Entity projectile, Entity source, Entity? target, World world);
        void Tick(Entity projectile, World world, float deltaTime);
    }
}
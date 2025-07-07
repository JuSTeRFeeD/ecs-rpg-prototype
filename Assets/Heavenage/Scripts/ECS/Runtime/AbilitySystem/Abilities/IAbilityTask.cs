using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities
{
    public interface IAbilityTask
    {
        /// <summary>
        /// Invoke every tick. Return true if the task is done.
        /// </summary>
        bool Tick(Entity activeAbility, Entity caster, Entity target, World world, float deltaTime);
    }
}
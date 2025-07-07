using System.Collections.Generic;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting
{
    public interface ITargetingStrategy
    {
        void OnStart(Entity activeAbility, World world);
        void Tick(Entity activeAbility, Entity caster, World world);
        void OnEnd(Entity activeAbility, World world);
        IEnumerable<Entity> GetTargets(Entity abilityEntity, World world);
    }
}
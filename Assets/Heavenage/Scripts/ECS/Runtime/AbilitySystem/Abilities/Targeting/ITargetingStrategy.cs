using System.Collections.Generic;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting
{
    public interface ITargetingStrategy
    {
        void OnStart(Entity caster, World world);
        void Tick(Entity caster, World world);
        void OnEnd(Entity caster, World world);
        IEnumerable<Entity> GetTargets(Entity caster, World world);
    }
}
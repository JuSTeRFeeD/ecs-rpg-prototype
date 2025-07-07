using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge.TargetingModifiers
{
    public interface ITargetingModifier
    {
        void Modify(Entity caster, World world, ITargetingStrategy strategy, float chargePercent);
    }
}
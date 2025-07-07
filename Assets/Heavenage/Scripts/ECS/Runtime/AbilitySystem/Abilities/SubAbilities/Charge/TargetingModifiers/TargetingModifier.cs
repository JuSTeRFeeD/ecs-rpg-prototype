using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge.TargetingModifiers
{
    public abstract class TargetingModifier : ScriptableObject, ITargetingModifier
    {
        public abstract void Modify(Entity caster, World world, ITargetingStrategy strategy, float chargePercent);
    }
}
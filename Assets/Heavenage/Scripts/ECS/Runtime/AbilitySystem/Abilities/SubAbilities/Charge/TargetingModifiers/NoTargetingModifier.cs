using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge.TargetingModifiers
{
    [CreateAssetMenu(fileName = "NoTargetingModifier", menuName = "RPG/SubAbility/Targeting/Modifiers/NoTargetingModifier", order = 0)]
    public class NoTargetingModifier : TargetingModifier
    {
        public override void Modify(Entity caster, World world, ITargetingStrategy strategy, float chargePercent) { }
    }
}
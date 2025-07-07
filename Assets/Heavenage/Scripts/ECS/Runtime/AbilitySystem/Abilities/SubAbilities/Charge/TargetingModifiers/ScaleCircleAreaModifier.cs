using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting.List;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge.TargetingModifiers
{
    [CreateAssetMenu(menuName = "RPG/SubAbility/Targeting/Modifiers/ScaleCircleAreaModifier")]
    public class ScaleCircleAreaModifier : TargetingModifier
    {
        public float minRadius = 0.5f;
        public float maxRadius = 2f;
        
        public override void Modify(Entity caster, World world, ITargetingStrategy strategy, float chargePercent)
        {
            if (strategy is CircleAreaStrategy circle)
            {
                circle.SetRadius(Mathf.Lerp(minRadius, maxRadius, chargePercent));
            }
        }
    }
}
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Waits
{
    [CreateAssetMenu(menuName = "RPG/SubAbility/Wait/WaitDuration")]
    public class WaitSubAbility : SubAbilityTask<WaitDurationAbilityTask>
    {
        public float duration;
    }

    public class WaitDurationAbilityTask : IAbilityTaskWithSetup
    {
        private float _duration;
        
        public void SetupFromSubAbility(SubAbility ability)
        {
            var data = (WaitSubAbility)ability;
            _duration = data.duration;
        }

        public bool Tick(Entity activeAbility, Entity caster, Entity target, World world, float deltaTime)
        {
            _duration -= deltaTime;
            return _duration <= 0f;
        }
    }
}
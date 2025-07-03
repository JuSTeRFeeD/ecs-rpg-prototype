using Heavenage.Scripts.AbilitySystem.Runtime.Abilities;
using Heavenage.Scripts.AbilitySystem.Runtime.Effects;

namespace Heavenage.Scripts.Abilities.Tests
{
    public class AbilityDamageSelfInstance : AbilityInstance
    {
        private GameplayEffectInstance _damageEffect;
        
        public AbilityDamageSelfInstance(GameplayEffect damageEffect)
        {
            _damageEffect = new GameplayEffectInstance(this, damageEffect);
        }

        public override void Activate()
        {
            base.Activate();
            Owner.ApplyEffect(_damageEffect);
        }
    }
}
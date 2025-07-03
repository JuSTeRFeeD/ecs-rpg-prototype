using Heavenage.Scripts.AbilitySystem.Runtime.Abilities;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Effects
{
    public class GameplayEffectInstance
    {
        private readonly AbilityInstance _abilityInstance;
        public readonly GameplayEffect Effect;

        private int _timesTicked;
        private float _elapsedTime;

        public bool IsExpired()
        {
            switch (Effect.DurationPolicy)
            {
                case EffectsDurationPolicy.Infinite:
                    return false;
                case EffectsDurationPolicy.Duration when Effect.Period > 0 &&
                                                         _timesTicked >= Effect.MaxTicks:
                case EffectsDurationPolicy.Duration when _elapsedTime > Effect.Duration:
                    return true;
                case EffectsDurationPolicy.Instant:
                default:
                    return true;
            }
        } 

        public GameplayEffectInstance(AbilityInstance abilityInstance, GameplayEffect effect)
        {
            _abilityInstance = abilityInstance;
            Effect = effect;
        }

        public void Tick(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (Effect.MaxTicks != 0 && Effect.Period != 0)
            {
                if (_elapsedTime > Effect.Period)
                {
                    _elapsedTime -= Effect.Period;
                    _timesTicked++;
                }
            }
        }
    }
}
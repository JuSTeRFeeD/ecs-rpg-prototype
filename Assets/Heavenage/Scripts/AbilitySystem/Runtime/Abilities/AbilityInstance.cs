using Heavenage.Scripts.AbilitySystem.Runtime.Components;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Abilities
{
    public class AbilityInstance
    {
        protected AbilitySystemComponent Owner { get; private set; }
        
        public bool IsActive { get; protected set; }

        public virtual void Initialize(AbilitySystemComponent owner)
        {
            Owner = owner;
        }

        public virtual bool CanActivate() => !IsActive;

        public virtual void Activate() => IsActive = true;
        
        public virtual void Tick(float deltaTime) { }

        public virtual void EndAbility()
        {
            IsActive = false;
            Owner.EndAbility(this);
        }
    }
}
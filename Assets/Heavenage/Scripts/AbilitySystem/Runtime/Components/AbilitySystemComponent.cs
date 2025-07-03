using System.Collections.Generic;
using Heavenage.Scripts.AbilitySystem.Runtime.Abilities;
using Heavenage.Scripts.AbilitySystem.Runtime.Attributes;
using Heavenage.Scripts.AbilitySystem.Runtime.Effects;
using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Components
{
    public class AbilitySystemComponent : MonoBehaviour, IAbilitySystemComponent
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor]
#endif
        [SerializeField] private AttributesPredefine attributesPredefine;

        [SerializeField] private Ability[] abilitiesPredefine;
        
        private readonly List<AbilityInstance> _abilities = new();
        private readonly List<AbilityInstance> _activeAbilities = new();
        private readonly List<AbilityInstance> _pendingAbilities = new();
        
        private readonly List<GameplayEffectInstance> _appliedEffects = new();
        
        public GameObject Owner { get; private set; }
        public AttributeSet AttributeSet { get; private set; }

        private void Awake()
        {
            AttributeSet = new AttributeSet(attributesPredefine.Attributes);

            foreach (var ability in abilitiesPredefine)
            {
                GiveAbility(ability);
            }
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            TickAbilities(dt);
            TickEffects(dt);
        }
        
        #region Abilities

        public void GiveAbility(Ability ability)
        {
            var abilityInstance = ability.CreateInstance();
            _abilities.Add(abilityInstance);
        }
        
        public void GiveAbility(AbilityInstance abilityInstance)
        {
            _abilities.Add(abilityInstance);
        }
        
        public void EndAbility(AbilityInstance abilityInstance)
        {
            _pendingAbilities.Add(abilityInstance);
        }

        public void TryActivateAbility(int index)
        {
            if (index < 0 || index >= _abilities.Count) return;
            
            var ability = _abilities[index];
            if (ability.CanActivate())
            {
                ability.Activate();
                _activeAbilities.Add(ability);
            }
        }

        private void TickAbilities(float dt)
        {
            foreach (var abilityInstance in _activeAbilities)
            {
                abilityInstance.Tick(dt);
            }

            foreach (var abilityInstance in _pendingAbilities)
            {
                _activeAbilities.Remove(abilityInstance);
            }
            _pendingAbilities.Clear();
        }

        #endregion
        
        #region Effects
        
        private void TickEffects(float dt)
        {
            var pendingToRemove = new List<GameplayEffectInstance>();
            foreach (var eff in _appliedEffects)
            {
                eff.Tick(dt);
                if (eff.IsExpired())
                {
                    pendingToRemove.Add(eff);   
                }
            }

            foreach (var instance in pendingToRemove)
            {
                _appliedEffects.Remove(instance);
            }
        }

        public void ApplyEffect(GameplayEffectInstance gameplayEffectInstance)
        {
            _appliedEffects.Add(gameplayEffectInstance);
        }

        #endregion
    }
}  

using Heavenage.Scripts.AbilitySystem.Runtime.Effects;
using Heavenage.Scripts.AbilitySystem.Runtime.Tags;
using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Abilities
{
    // [CreateAssetMenu(fileName = "New Ability", menuName = "Ability System/Ability", order = 0)]
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] private string abilityName;
        [SerializeField] private GameplayEffect cooldown;
        [SerializeField] private GameplayTagConfig activationTag;
        
        public string AbilityName => abilityName;
        public GameplayTagConfig ActivationTag => activationTag;
        public GameplayEffect Cooldown => cooldown;

        public abstract AbilityInstance CreateInstance();
    }
}
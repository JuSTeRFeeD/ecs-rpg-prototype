using Heavenage.Scripts.AbilitySystem.Runtime.Abilities;
using Heavenage.Scripts.AbilitySystem.Runtime.Effects;
using UnityEngine;

namespace Heavenage.Scripts.Abilities.Tests
{
    [CreateAssetMenu(fileName = "Damage Self Ability", menuName = "Abilities/Test/Damage Self Ability")]
    public class AbilityDamageSelf : Ability
    {
        [SerializeField] private GameplayEffect damageEffect;
        
        public override AbilityInstance CreateInstance()
        {
            return new AbilityDamageSelfInstance(damageEffect);
        }
    }
}
using Heavenage.Scripts.AbilitySystem.Runtime.Attributes;
using Heavenage.Scripts.AbilitySystem.Runtime.Components;
using UnityEngine;

namespace Heavenage.Scripts.Characters
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private AbilitySystemComponent abilitySystemComponent;
        [SerializeField] private AttributeDefinition healthAttribute;

        private void Start()
        {
            var health = abilitySystemComponent.AttributeSet.GetAttributeByDefinition(healthAttribute);
        }
    }
}
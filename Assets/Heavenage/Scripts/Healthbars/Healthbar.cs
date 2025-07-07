using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Heavenage.Scripts.ECS.Runtime.Views;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Heavenage.Scripts.Healthbars
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private EntityView targetEntityView;
        [SerializeField] private AttributeType healthAttribute;
        [Space]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image fillImage;

        private Stash<AttributeComponent> _attributeStash;
        
        private void Start()
        {
            _attributeStash = StashRegistry.GetStash<AttributeComponent>();
        }

        private void Update()
        {
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            if (!_attributeStash.Has(targetEntityView.Entity)) return;

            ref readonly var attributeComponent = ref _attributeStash.Get(targetEntityView.Entity);
            var healthAttr = attributeComponent.AttributeMap[healthAttribute.Hash];

            var cur = healthAttr.currentValue;
            var max = healthAttr.FinalValue;
            healthText.text = $"{(int)cur} / {(int)max}";
            fillImage.fillAmount = cur / max;
        }
    }
}
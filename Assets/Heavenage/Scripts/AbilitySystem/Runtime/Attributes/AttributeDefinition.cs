using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Attributes
{
    [CreateAssetMenu(menuName = "Ability System/Attributes/New Attribute")]
    public class AttributeDefinition : ScriptableObject
    {
        [SerializeField] private string attributeName;
        [Space]
        [SerializeField] private float minValue = float.MinValue;
        [SerializeField] private float maxValue = float.MaxValue;

        [ContextMenu("Reset MinMax")]
        private void Reset()
        {
            minValue = float.MinValue;
            maxValue = float.MaxValue;
        }
        
        public string AttributeName => attributeName;
        public float MinValue => minValue;
        public float MaxValue => maxValue;
    }
}
using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Attributes
{
    [CreateAssetMenu(menuName = "Ability System/Attributes/AttributesPredefine")]
    public class AttributesPredefine : ScriptableObject
    {
        [SerializeField] private AttributeSetup[] attributes;
        
        public AttributeSetup[] Attributes => attributes;
    }
}
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes
{
    [CreateAssetMenu(menuName = "RPG/AttributeType")]
    public class AttributeType : ScriptableObject
    {
        [SerializeField] private string id;

        public string Id => id;
        public int Hash => id.GetHashCode();
    }
}
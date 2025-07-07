using System.Collections.Generic;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes
{
    [CreateAssetMenu(menuName = "RPG/Attribute Start Set")]
    public class AttributeStartSet : ScriptableObject
    {
        [System.Serializable]
        public class Entry {
            public AttributeType type;
            public float baseValue = 0;
        }

        public List<Entry> attributes = new List<Entry>();
    }
}
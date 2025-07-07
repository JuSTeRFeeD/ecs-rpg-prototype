using System.Collections.Generic;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes
{
    public static class AttributeCache
    {
        private static Dictionary<string, AttributeType> _idToType;
        private static Dictionary<AttributeType, int> _typeToHash;

        static AttributeCache()
        {
            _idToType = new Dictionary<string, AttributeType>();
            _typeToHash = new Dictionary<AttributeType, int>();
        }

        public static int GetHash(AttributeType type)
        {
            if (!_typeToHash.TryGetValue(type, out var hash))
            {
                hash = type.Hash;
                _typeToHash[type] = hash;
            }
            return hash;
        }

        public static void Register(AttributeType type)
        {
            _idToType.TryAdd(type.Id, type);
        }
        
        public static AttributeType Get(string id) => _idToType.GetValueOrDefault(id);
        
        public static IEnumerable<AttributeType> GetAllTypes() => _idToType.Values;
    }
}

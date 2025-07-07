using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes
{
    public static class AttributeUtils
    {
        public static AttributeComponent CrateAttributesByStartSet(AttributeStartSet startSet)
        {
            var map = new Dictionary<int, AttributeEntry>();

            foreach (var entry in startSet.attributes)
            {
                if (entry.type == null) continue;

                var hash = entry.type.Hash;
                map[hash] = new AttributeEntry
                {
                    baseValue = entry.baseValue,
                    multiplier = 0,
                    bonusValue = 0,
                    currentValue = entry.baseValue
                };
            }
            
            return new AttributeComponent { AttributeMap = map };
        }
        
        public static void SetBase(Entity entity, AttributeType type, float value)
        {
            ref var attr = ref StashRegistry.GetStash<AttributeComponent>().Get(entity);
            var hash = AttributeCache.GetHash(type);
            if (!attr.AttributeMap.TryGetValue(hash, out var entry))
            {
                entry = new AttributeEntry();
            }

            entry.baseValue = value;
            attr.AttributeMap[hash] = entry;
        }

        public static float GetFinal(Entity entity, AttributeType type)
        {
            ref var attr = ref StashRegistry.GetStash<AttributeComponent>().Get(entity);
            var hash = AttributeCache.GetHash(type);
            return attr.AttributeMap.TryGetValue(hash, out var entry) ? entry.FinalValue : 0f;
        }

        public static void Modify(Entity entity, AttributeType type, float bonus, float multiplier)
        {
            ref var attr = ref StashRegistry.GetStash<AttributeComponent>().Get(entity);
            var hash = AttributeCache.GetHash(type);
            if (!attr.AttributeMap.TryGetValue(hash, out var entry))
            {
                entry = new AttributeEntry();
            }

            entry.bonusValue += bonus;
            entry.multiplier += multiplier;
            attr.AttributeMap[hash] = entry;
        }
    }
}
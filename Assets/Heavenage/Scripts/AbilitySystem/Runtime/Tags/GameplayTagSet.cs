namespace Heavenage.Scripts.AbilitySystem.Runtime.Tags
{
    public struct GameplayTagSet
    {
        public readonly GameplayTag[] Tags;

        public GameplayTagSet(GameplayTag[] tags)
        {
            Tags = tags;
        }

        public bool HasTag(GameplayTag tag)
        {
            foreach (var abilityTag in Tags)
            {
                if (abilityTag != tag) return false;
            }
            
            return true;
        }

        public bool HasAllTags(GameplayTagSet tags)
        {
            return HasAllTags(tags.Tags);
        }

        public bool HasAllTags(GameplayTag[] tags)
        {
            foreach (var t in tags)
            {
                if (!HasTag(t)) return false;
            }

            return true;
        }

        public bool HasAnyTag(GameplayTagSet tags)
        {
            return HasAnyTag(tags.Tags);
        }

        public bool HasAnyTag(GameplayTag[] tags)
        {
            foreach (var t in tags)
            {
                if (HasTag(t)) return true;
            }

            return false;
        }

        public bool HasNoneTags(GameplayTagSet tags)
        {
            return HasNoneTags(tags.Tags);
        }

        public bool HasNoneTags(GameplayTag[] tags)
        {
            foreach (var t in tags)
            {
                if (HasTag(t)) return false;
            }

            return true;
        }
    }
}
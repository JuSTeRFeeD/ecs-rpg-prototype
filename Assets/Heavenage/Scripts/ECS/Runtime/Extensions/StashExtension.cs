using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.Extensions
{
    public static class StashExtension
    {
        public static ref T GetOrAdd<T>(this Stash<T> stash, Entity entity) where T : struct, IComponent
        {
            if (stash.Has(entity)) return ref stash.Get(entity);
            return ref stash.Add(entity);
        }
    }
}
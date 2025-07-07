using System;
using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Extensions
{
    public static class StashRegistry
    {
        private static readonly Dictionary<Type, object> Stashes = new();

        public static void RegisterStashes(IContainerBuilder builder, World world)
        {
            Stashes.Clear();
            var componentTypes = GetComponentTypes();

            foreach (var type in componentTypes)
            {
                var stash = world.GetReflectionStash(type);
                var stashType = typeof(Stash<>).MakeGenericType(type);
                Stashes[type] = stash;
                builder.RegisterInstance(stash, stashType);
            }
        }

        public static Stash<T> GetStash<T>() where T : struct, IComponent
        {
            if (Stashes.TryGetValue(typeof(T), out var stash))
                return (Stash<T>)stash;
        
            throw new KeyNotFoundException($"Stash<{typeof(T).Name}> not registered!");
        } 
        
        private static IEnumerable<Type> GetComponentTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsValueType && typeof(IComponent).IsAssignableFrom(t));
        }
    }
}
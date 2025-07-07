using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Extensions
{
    public static class SystemGroupExtension
    {
        private static IObjectResolver _resolver;
        
        public static void SetResolver(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSystem<TSystem>(this SystemsGroup group, bool enabled = true) 
            where TSystem : class, ISystem, new()
        {
            var system = new TSystem();
            _resolver.Inject(system);
            group.AddSystem(system, enabled);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddInitializer<TInitializer>(this SystemsGroup group) 
            where TInitializer : class, IInitializer, new()
        {
            var initializer = new TInitializer();
            _resolver.Inject(initializer);
            group.AddInitializer(initializer);
        }
    }
}
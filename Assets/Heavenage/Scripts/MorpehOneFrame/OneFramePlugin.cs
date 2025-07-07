using System;
using System.Linq;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Scripting;

namespace Heavenage.Scripts.MorpehOneFrame
{
    [Preserve]
    public class OneFramePlugin : IWorldPlugin
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
            WorldPluginsExtensions.AddWorldPlugin(new OneFramePlugin());
        }
        
        internal OneFramePlugin() { }
        
        [Preserve]
        public void Initialize(World world)
        {
            // Register system
            var systemsGroups = world.CreateSystemsGroup();
            systemsGroups.AddSystem(new OneFrameCleanupSystem());
            world.AddPluginSystemsGroup(systemsGroups);
            
            // Register components automatically
            var oneFrameTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IOneFrameComponent).IsAssignableFrom(t) && t.IsValueType && !t.IsInterface)
                .ToArray();
            
            var registerMethod = typeof(OneFrameRegister).GetMethod("RegisterOneFrame", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var register = OneFrameRegister.GetFor(world);
            
            foreach (var type in oneFrameTypes)
            {
                var genericMethod = registerMethod.MakeGenericMethod(type);
                genericMethod.Invoke(register, null);
            }
        }

        public void Deinitialize(World world) { }
    }
}
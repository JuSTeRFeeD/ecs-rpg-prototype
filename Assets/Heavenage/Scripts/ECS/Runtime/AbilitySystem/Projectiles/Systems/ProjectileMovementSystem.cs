using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Projectiles.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Projectiles.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileMovementSystem : ISystem
    {
        [Inject] private Stash<ActiveProjectileComponent> _activeProjectileStash;
        
        public World World { get; set; }

        private Filter _filter;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<ActiveProjectileComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var data = ref _activeProjectileStash.Get(entity);
                data.Logic.Tick(data.Projectile, World, deltaTime);
            }
        }

        public void Dispose() { }
    }
}
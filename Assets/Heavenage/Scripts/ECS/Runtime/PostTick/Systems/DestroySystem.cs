using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.PostTick.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroySystem : ISystem
    {
        [Inject] private Stash<LinkedEntitiesComponent> _linkedEntitiesStash;
        
        public World World { get; set; }

        private Filter _toDestroyFilter;

        public void OnAwake()
        {
            _toDestroyFilter = World.Filter
                .With<ToDestroyTag>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _toDestroyFilter)
            {
                if (_linkedEntitiesStash.Has(entity))
                {
                    ref var linked = ref _linkedEntitiesStash.Get(entity);
                    foreach (var linkedValue in linked.Values)
                    {
                        World.RemoveEntity(linkedValue);
                    }
                }
                World.RemoveEntity(entity);
            }
        }

        public void Dispose()
        {
        }
    }
}
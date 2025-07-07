using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.PostTick.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyOverTimeSystem : ISystem
    {
        [Inject] private Stash<DestroyOverTimeComponent> _destroyOverTimeStash;
        [Inject] private Stash<ToDestroyTag> _toDestroyStash;
        
        public World World { get; set; }

        private Filter _filter;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<DestroyOverTimeComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var destroy = ref _destroyOverTimeStash.Get(entity);
                destroy.EstimateTime -= deltaTime;
                
                if (destroy.EstimateTime > 0f) continue;
                
                _toDestroyStash.Set(entity, new ToDestroyTag());
            }
        }

        public void Dispose()
        {
        }
    }
}
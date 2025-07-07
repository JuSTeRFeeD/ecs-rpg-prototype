using Heavenage.Scripts.ECS.Runtime.Authoring.Characters;
using Heavenage.Scripts.ECS.Runtime.Spawning.Components;
using Heavenage.Scripts.ECS.Runtime.Utils;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Spawning.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SpawnCharacterSystem : ISystem
    {
        [Inject] private Stash<CharacterAuthoringComponent> _characterAuthoringStash;
        [Inject] private Stash<SpawningEntityComponent> _spawningEntityStash;
        
        public World World { get; set; }

        private Filter _filter;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterAuthoringComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterAuthoring = ref _characterAuthoringStash.Get(entity);
                
                var spawnedEntity = World.CreateEntity();
                spawnedEntity.InstantiateView(characterAuthoring.viewPrefab);

                _spawningEntityStash.Add(entity, new SpawningEntityComponent
                {
                    Entity = spawnedEntity
                });
            }
        }

        public void Dispose()
        {
        }
    }
}
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components;
using Heavenage.Scripts.ECS.Runtime.Authoring.Characters;
using Heavenage.Scripts.ECS.Runtime.Spawning.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Spawning.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SetupSpawningAttributesSystem : ISystem
    {
        [Inject] private Stash<AttributeComponent> _attributesStash;
        [Inject] private Stash<AttributesAuthoringComponent> _attributesAuthoringStash;
        [Inject] private Stash<SpawningEntityComponent> _spawningEntityStash;
        
        public World World { get; set; }

        private Filter _filter;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<AttributesAuthoringComponent>()
                .With<SpawningEntityComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var authoring = ref _attributesAuthoringStash.Get(entity);
                ref var spawningEntity = ref _spawningEntityStash.Get(entity).Entity;
                
                var attributes = AttributeUtils.CrateAttributesByStartSet(authoring.attributeStartSet);
                _attributesStash.Add(spawningEntity, attributes);
            }
        }

        public void Dispose()
        {
        }
    }
}
using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
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
    public sealed class SetupSpawningAbilitiesSystem : ISystem
    {
        [Inject] private Stash<AbilitiesAuthoringComponent> _abilitiesAuthoringStash;
        [Inject] private Stash<AbilitiesComponent> _abilitiesStash;
        [Inject] private Stash<AbilityComponent> _abilityStash;
        [Inject] private Stash<SpawningEntityComponent> _spawningEntityStash;
        
        public World World { get; set; }

        private Filter _filter;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<SpawningEntityComponent>()
                .With<AbilitiesAuthoringComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref readonly var spawningEntity = ref _spawningEntityStash.Get(entity).Entity;
                ref var abilities = ref _abilitiesAuthoringStash.Get(entity).Abilities;
                
                var abilityEntities = new List<Entity>();
                
                foreach (var abilityDefinition in abilities)
                {
                    var abilityEntity = World.CreateEntity();
                    
                    _abilityStash.Add(abilityEntity, new AbilityComponent
                    {
                        AbilityDefinition = abilityDefinition
                    });
                    
                    abilityEntities.Add(abilityEntity);
                    LinkedEntityUtils.Link(spawningEntity, abilityEntity);
                }
                
                _abilitiesStash.Add(spawningEntity, new AbilitiesComponent
                {
                    AbilityEntities = abilityEntities
                });
            }
        }

        public void Dispose()
        {
        }
    }
}
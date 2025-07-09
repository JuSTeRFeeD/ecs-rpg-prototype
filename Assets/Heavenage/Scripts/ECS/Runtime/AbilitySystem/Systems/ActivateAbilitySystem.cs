using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ActivateAbilitySystem : ISystem
    {
        [Inject] private Stash<UseAbilityRequest> _useAbilityStash;
        [Inject] private Stash<AbilityComponent> _abilityComponentStash;
        [Inject] private Stash<ActiveAbilityComponent> _activeAbilityStash;
        [Inject] private Stash<OwnerComponent> _ownerStash;
        [Inject] private Stash<OriginalAbilityInProgressTag> _originalAbilityInProgressStash;
        
        public World World { get; set; }

        private Filter _filter;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<UseAbilityRequest>()
                .With<AbilityComponent>()
                .Without<OriginalAbilityInProgressTag>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref readonly var activateAbility = ref _useAbilityStash.Get(entity);
                ref readonly var abilityData = ref _abilityComponentStash.Get(entity);

                // Create active ability entity to execute
                var activeAbilityEntity = World.CreateEntity();
                _ownerStash.Add(activeAbilityEntity, new OwnerComponent
                {
                    Value = _ownerStash.Get(entity).Value
                });
                _activeAbilityStash.Add(activeAbilityEntity, new ActiveAbilityComponent
                {
                    OriginalAbilityEntity = entity,
                    Caster = activateAbility.Caster,
                    Tasks = abilityData.AbilityDefinition.CreateAbilityTasks(),
                    CurrentStep = 0,
                });
                
                Debug.Log("activate ability " + abilityData.AbilityDefinition.AbilityName);
                
                // Mark original ability as in progress
                _originalAbilityInProgressStash.Set(entity, new OriginalAbilityInProgressTag
                {
                    ActiveAbilityInProgress = activeAbilityEntity
                });

                _useAbilityStash.Remove(entity);
            }
        }

        public void Dispose()
        {
        }
    }
}
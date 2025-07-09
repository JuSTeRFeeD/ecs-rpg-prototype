using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityExecutionSystem : ISystem
    {
        [Inject] private Stash<ActiveAbilityComponent> _activeAbilityStash;
        [Inject] private Stash<OriginalAbilityInProgressTag> _originalAbilityInProgressStash;
        [Inject] private Stash<ToDestroyTag> _toDestroyStash;
        
        public World World { get; set; }
        
        private Filter _activeAbilitiesFilter;

        public void OnAwake()
        {
            _activeAbilitiesFilter = World.Filter
                .With<ActiveAbilityComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _activeAbilitiesFilter)
            {
                ref var activeAbility = ref _activeAbilityStash.Get(entity);
                var tasks = activeAbility.Tasks;

                // End ability
                if (activeAbility.CurrentStep >= tasks.length)
                {
                    Debug.Log($"End of active ability {entity.Id} IsDisposed: {World.IsDisposed(entity)} | original: {activeAbility.OriginalAbilityEntity.Id}");
                    Debug.Log($"Original Ability Disposed? {World.IsDisposed(activeAbility.OriginalAbilityEntity)}");
                    // TODO: FIX: End Ability called multiple times and second time original ability is disposed! why?
                    if (!World.IsDisposed(activeAbility.OriginalAbilityEntity))
                    {
                        _originalAbilityInProgressStash.Remove(activeAbility.OriginalAbilityEntity);
                    }
                    _toDestroyStash.Add(entity);
                    continue;
                }

                var task = tasks[activeAbility.CurrentStep];
                if (task.Tick(entity, activeAbility.Caster, activeAbility.Target, World, deltaTime))
                {
                    activeAbility.CurrentStep++;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
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
        
        public World World { get; set; }

        private Filter _filter;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<UseAbilityRequest>()
                .With<AbilityComponent>()
                .Without<ActiveAbilityComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref readonly var activateAbility = ref _useAbilityStash.Get(entity);
                
                // TODO: somewhere check if ability is already active && is not on cooldown && requirements are met
                // before activating
                
                ref readonly var ability = ref _abilityComponentStash.Get(entity);
                
                _activeAbilityStash.Add(entity, new ActiveAbilityComponent
                {
                    Caster = activateAbility.Caster,
                    Tasks = ability.AbilityDefinition.CreateAbilityTasks(),
                    CurrentStep = 0,
                    Timer = 0f,
                });

                _useAbilityStash.Remove(entity);
            }
        }

        public void Dispose()
        {
        }
    }
}
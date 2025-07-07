using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SetAbilityInputReleasedSystem : ISystem
    {
        [Inject] private Stash<OwnerComponent> _ownerStash;
        [Inject] private Stash<UseAbilityRequest> _useAbilityStash;
        [Inject] private Stash<AbilityComponent> _abilityComponentStash;
        [Inject] private Stash<ActiveAbilityComponent> _activeAbilityStash;
        [Inject] private Stash<OriginalAbilityInProgressTag> _originalAbilityInProgressStash;
        [Inject] private Stash<InputReleasedTag> _inputReleasedStash;
        
        public World World { get; set; }

        private Filter _filter;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<AbilityComponent>()
                .With<InputReleasedTag>()
                .With<OriginalAbilityInProgressTag>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref readonly var inProgress = ref _originalAbilityInProgressStash.Get(entity);
                _inputReleasedStash.Set(inProgress.ActiveAbilityInProgress);
            }
        }

        public void Dispose()
        {
        }
    }
}
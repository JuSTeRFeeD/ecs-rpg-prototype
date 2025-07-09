using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Player.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Player.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUseAbilitySystem : ISystem
    {
        [Inject] private Stash<AbilitiesComponent> _abilitiesStash;
        [Inject] private Stash<UseAbilityRequest> _useAbilityStash;
        [Inject] private Stash<InputReleasedTag> _inputReleasedStash;
        
        public World World { get; set; }

        private Filter _playerFilter;

        public void OnAwake()
        {
            _playerFilter = World.Filter
                .With<PlayerTag>()
                .With<AbilitiesComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _playerFilter)
            {
                if (Input.GetMouseButtonDown(1))
                    TryActivateAbility(entity, 0, false);
                else if (Input.GetMouseButtonUp(1))
                    TryActivateAbility(entity, 0, true);
            }
        }

        private void TryActivateAbility(Entity character, int abilityIdx, bool isReleased)
        {
            ref var abilities = ref _abilitiesStash.Get(character);
            
            if (abilities.AbilityEntities == null || abilityIdx > abilities.AbilityEntities.Count || abilityIdx < 0)
                return;

            var abilityEntity = abilities.AbilityEntities[abilityIdx];

            if (!isReleased)
            {
                _useAbilityStash.Set(abilityEntity, new UseAbilityRequest { Caster = character, });
            }
            else
            {
                _inputReleasedStash.Set(abilityEntity);
            }
        }

        public void Dispose() { }
    }
}
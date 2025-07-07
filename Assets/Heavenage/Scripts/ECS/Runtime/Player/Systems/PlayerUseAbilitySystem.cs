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
        
        public World World { get; set; }

        private Filter _filter;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerTag>()
                .With<AbilitiesComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    TryActivateAbility(entity, 0);
                }
            }
        }

        private void TryActivateAbility(Entity entity, int index)
        {
            ref var abilities = ref _abilitiesStash.Get(entity);
            
            if (abilities.AbilityEntities == null || index > abilities.AbilityEntities.Count || index < 0)
                return;

            var abilityEntity = abilities.AbilityEntities[index];
            _useAbilityStash.Set(abilityEntity, new UseAbilityRequest
            {
                Caster = entity,
                Target = default,
            });
            Debug.Log($"Used ability {index}");
        }

        public void Dispose() { }
    }
}
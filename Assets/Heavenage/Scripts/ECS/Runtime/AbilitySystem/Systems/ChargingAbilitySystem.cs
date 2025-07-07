using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Charge;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Systems
{
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    public sealed class ChargingAbilitySystem : ISystem
    {
        [Inject] private Stash<ChargingAbilityComponent> _chargingAbilityStash;
        
        public World World { get; set; }

        private Filter _filter;
        
        public void OnAwake()
        {
            _filter = World.Filter.With<ChargingAbilityComponent>().Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var charge = ref _chargingAbilityStash.Get(entity);
                
                charge.ElapsedTime = Mathf.Clamp(charge.ElapsedTime, 0, charge.MaxChargeTime);

                // todo: or input release
                var execute = charge.ElapsedTime >= charge.MaxChargeTime;

                if (!execute) return;
            }
        }

        public void Dispose()
        {
        }
    }
}
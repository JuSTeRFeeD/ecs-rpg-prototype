using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityExecutionSystem : ISystem
    {
        [Inject] private Stash<ActiveAbilityComponent> _activeAbilityStash;
        
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
                if (activeAbility.CurrentStep >= tasks.Count)
                {
                    _activeAbilityStash.Remove(entity);
                    continue;
                }

                var task = tasks[activeAbility.CurrentStep];
                if (task.Tick(activeAbility.Caster, activeAbility.Target, World, deltaTime))
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
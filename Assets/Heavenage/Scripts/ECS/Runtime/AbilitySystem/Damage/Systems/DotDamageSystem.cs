using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Damage.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DoTDamageSystem : ISystem
    {
        [Inject] private Stash<DotEffectComponent> _dotEffectStash;
        [Inject] private Stash<AttributeComponent> _attributeStash;
        [Inject] private Stash<ToDestroyTag> _toDestroyStash;
        
        public World World { get; set; }

        private Filter _filter;

        private int _healthHash;

        public void OnAwake()
        {
            _healthHash = "Health".GetHashCode();

            _filter = World.Filter
                .With<DotEffectComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var dot = ref _dotEffectStash.Get(entity);
                
                dot.Elapsed += deltaTime;

                if (dot.Elapsed >= dot.TickInterval)
                {
                    dot.Elapsed -= dot.TickInterval;

                    if (_attributeStash.Has(dot.Target))
                    {
                        ref var targetAttributes = ref _attributeStash.Get(dot.Target);
                        if (targetAttributes.AttributeMap.TryGetValue(_healthHash, out var health))
                        {
                            Debug.Log("dot damage");
                            health.currentValue -= dot.DamagePerTick;
                            targetAttributes.AttributeMap[_healthHash] = health;
                        }
                    }
                }

                dot.Duration -= deltaTime;
                if (dot.Duration <= 0f)
                {
                    _toDestroyStash.Add(entity);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
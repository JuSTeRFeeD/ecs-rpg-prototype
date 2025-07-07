using System.Linq;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components;
using Scellecs.Morpeh;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Systems
{
    public sealed class AttributeModifierSystem : ISystem
    {
        [Inject] private Stash<AttributeComponent> _attributeStash;
        [Inject] private Stash<AttributeModifiersComponent> _attributeModifierStash;

        public World World { get; set; }

        private Filter _filter;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<AttributeModifiersComponent>()
                .With<AttributeComponent>()
                .Build();

            var testEnt = World.CreateEntity();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var attributeComponent = ref _attributeStash.Get(entity);
                ref var modifiersComponent = ref _attributeModifierStash.Get(entity);

                // Сбросим все модификаторы перед пересчётом
                foreach (var key in attributeComponent.AttributeMap.Keys.ToList())
                {
                    var entry = attributeComponent.AttributeMap[key];
                    entry.bonusValue = 0f;
                    entry.multiplier = 0f;
                    attributeComponent.AttributeMap[key] = entry;
                }

                // Применяем все активные модификаторы
                for (var i = modifiersComponent.Modifiers.Count - 1; i >= 0; i--)
                {
                    var modifier = modifiersComponent.Modifiers[i];
                    modifier.TimeRemaining -= deltaTime;

                    if (modifier.TimeRemaining <= 0f)
                    {
                        modifiersComponent.Modifiers.RemoveAt(i);
                        continue;
                    }

                    if (attributeComponent.AttributeMap.TryGetValue(modifier.AttributeId, out var entry))
                    {
                        entry.bonusValue += modifier.BonusDelta;
                        entry.multiplier += modifier.MultiplierDelta;
                        attributeComponent.AttributeMap[modifier.AttributeId] = entry;
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
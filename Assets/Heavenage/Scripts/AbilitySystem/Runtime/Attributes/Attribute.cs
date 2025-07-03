using System;
using System.Collections.Generic;
using Heavenage.Scripts.AbilitySystem.Runtime.Attributes.Modifiers;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Attributes
{
    public class Attribute
    {
        public readonly AttributeDefinition Definition;
        
        public float BaseValue { get; private set; }
        public float CurrentValue  { get; private set; }

        private readonly List<AttributeModifierHandle> _modifiers = new();

        public Attribute(AttributeSetup setup)
        {
            Definition = setup.attribute;
            BaseValue = setup.value;
            CurrentValue = setup.value;
        }
        
        public float CalculateFinalValue()
        {
            var value = BaseValue;
            foreach (var handle in _modifiers)
            {
                if (handle.Modifier.Operation == ModifierOperation.Add)
                    value += handle.Modifier.Value;
            }
            foreach (var handle in _modifiers)
            {
                if (handle.Modifier.Operation == ModifierOperation.Multiply)
                    value *= handle.Modifier.Value;
            }
            foreach (var handle in _modifiers)
            {
                if (handle.Modifier.Operation == ModifierOperation.Override)
                    value = handle.Modifier.Value;
            }

            return value;
        }
        
        public void AddModifier(AttributeModifierHandle modifier)
        {
            if (modifier.Modifier.Type == ModifierType.Instant)
            {
                switch (modifier.Modifier.Operation)
                {
                    case ModifierOperation.Add:
                        CurrentValue += modifier.Modifier.Value;
                        break;
                    case ModifierOperation.Multiply:
                        CurrentValue *= modifier.Modifier.Value;
                        break;
                    case ModifierOperation.Override:
                        CurrentValue = modifier.Modifier.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _modifiers.Add(modifier);
            }
        }

        public void RemoveModifier(AttributeModifierHandle modifier)
        {
            _modifiers.Remove(modifier);
        }

        public void RemoveAllModifiersBySource(uint sourceId)
        {
            _modifiers.RemoveAll(handle => handle.SourceId == sourceId);
        }
    }
}
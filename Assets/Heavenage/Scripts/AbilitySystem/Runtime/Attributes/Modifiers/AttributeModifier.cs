namespace Heavenage.Scripts.AbilitySystem.Runtime.Attributes.Modifiers
{
    public enum ModifierOperation
    {
        Add = 0,
        Multiply = 1,
        Override = 2,
    }

    public enum ModifierType
    {
        Instant,
        Infinite
    }
    
    public struct AttributeModifier
    {
        public readonly float Value;
        public readonly ModifierOperation Operation;
        public readonly ModifierType Type;

        public AttributeModifier(float value, ModifierOperation operation, ModifierType type = ModifierType.Instant)
        {
            Value = value;
            Operation = operation;
            Type = type;
        }
    }
}
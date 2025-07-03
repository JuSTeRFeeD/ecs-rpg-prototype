namespace Heavenage.Scripts.AbilitySystem.Runtime.Attributes.Modifiers
{
    public struct AttributeModifierHandle
    {
        public readonly AttributeModifier Modifier;
        public readonly uint SourceId;

        public AttributeModifierHandle(AttributeModifier modifier, uint sourceId)
        {
            Modifier = modifier;
            SourceId = sourceId;
        }
    }
}
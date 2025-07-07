using System.Collections.Generic;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components
{
    public struct AttributeModifier {
        public int AttributeId;        // ID, к которому применяется
        public float BonusDelta;       // Модификатор бонуса
        public float MultiplierDelta;  // Модификатор множителя
        public float Duration;         // Время действия
        public float TimeRemaining;    // Сколько осталось
    }
    
    public struct AttributeModifiersComponent : IComponent
    {
        public List<AttributeModifier> Modifiers;
    }
}
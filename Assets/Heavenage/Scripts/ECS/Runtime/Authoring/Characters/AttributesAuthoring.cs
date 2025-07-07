using System;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Heavenage.Scripts.ECS.Runtime.Authoring.Characters
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct AttributesAuthoringComponent : IComponent
    {
        public AttributeStartSet attributeStartSet;
    }
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class AttributesAuthoring : MonoProvider<AttributesAuthoringComponent>
    {
    }
}
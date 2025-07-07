using System;
using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Heavenage.Scripts.ECS.Runtime.Authoring.Characters
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct AbilitiesAuthoringComponent : IComponent
    {
        public List<AbilityDefinition> Abilities;
    }
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class AbilitiesAuthoring : MonoProvider<AbilitiesAuthoringComponent>
    {
    }
}
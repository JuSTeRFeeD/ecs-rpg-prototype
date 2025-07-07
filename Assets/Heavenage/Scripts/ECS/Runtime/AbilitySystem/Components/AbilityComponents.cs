using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components
{
    public struct AbilitiesComponent : IComponent
    {
        public List<Entity> AbilityEntities;
    }

    public struct AbilityComponent : IComponent
    {
        public AbilityDefinition AbilityDefinition;
    }
    
    public struct UseAbilityRequest : IComponent
    {
        public Entity Caster;
        public Entity Target;
    }

    public struct ActiveAbilityComponent : IComponent
    {
        public Entity Caster;
        public Entity Target;
        public int CurrentStep;
        public float Timer;
        public List<IAbilityTask> Tasks;
    }
}
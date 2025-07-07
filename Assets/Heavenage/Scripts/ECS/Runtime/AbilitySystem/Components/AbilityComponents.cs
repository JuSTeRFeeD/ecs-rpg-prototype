using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities;
using Heavenage.Scripts.MorpehOneFrame;
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
    
    public struct UseAbilityRequest : IOneFrameComponent
    {
        public Entity Caster;
        public bool IsReleased;
    }

    public struct InputReleasedTag : IOneFrameComponent { }

    public struct OriginalAbilityInProgressTag : IComponent
    {
        public Entity ActiveAbilityInProgress;
    }

    public struct ActiveAbilityComponent : IComponent
    {
        /// Original ability entity
        public Entity OriginalAbilityEntity; 
        public Entity Caster;
        public Entity Target;
        public int CurrentStep;
        public List<IAbilityTask> Tasks;
    }
}
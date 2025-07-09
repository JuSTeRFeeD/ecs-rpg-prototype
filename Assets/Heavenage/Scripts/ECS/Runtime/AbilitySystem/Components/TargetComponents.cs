using System.Collections.Generic;
using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components
{
    public struct ResolvedTargetsComponent : IComponent
    {
        public List<Entity> Targets;
    }
}
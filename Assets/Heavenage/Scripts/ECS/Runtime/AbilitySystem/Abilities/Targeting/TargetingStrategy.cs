using Heavenage.Scripts.ECS.Runtime.Views;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract ITargetingStrategy CreateTargetingStrategy(EntityView indicatorView);
    }
}
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities
{
    public abstract class SubAbility : ScriptableObject
    {
        public abstract IAbilityTask CreateTask();
    }
}
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "RPG/Ability Definition")]
    public class AbilityDefinition : ScriptableObject
    {
        [SerializeField] private string abilityName;
        [InlineEditor]
        [SerializeField] private List<SubAbility> steps;
        
        public string AbilityName => abilityName;
        public List<SubAbility> Steps => steps;

        public List<IAbilityTask> CreateAbilityTasks()
        {
            var list = new List<IAbilityTask>(steps.Count);
            foreach (var subAbility in steps)
            {
                list.Add(subAbility.CreateTask());
            }
            return list;
        }
    }
}
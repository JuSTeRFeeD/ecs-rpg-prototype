using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh.Collections;
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

        public FastList<IAbilityTask> CreateAbilityTasks()
        {
            var list = new FastList<IAbilityTask>(steps.Count);
            foreach (var subAbility in steps)
            {
                list.Add(subAbility.CreateTask());
            }
            return list;
        }
    }
}
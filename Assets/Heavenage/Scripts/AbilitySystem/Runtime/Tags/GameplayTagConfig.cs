using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Tags
{
    [CreateAssetMenu(fileName = "Gameplay Tag Config", menuName = "Ability System/Gameplay Tag")]
    public class GameplayTagConfig : ScriptableObject
    {
        [SerializeField] private string tagName;
    
        public GameplayTag TagInstance => new(tagName);
    }
}

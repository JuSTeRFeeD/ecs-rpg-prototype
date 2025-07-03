using System.Collections.Generic;
using Heavenage.Scripts.AbilitySystem.Runtime.Tags;
using UnityEngine;

namespace Heavenage.Scripts.AbilitySystem.Runtime
{
    public class AbilitySystemManager : MonoBehaviour
    {
        public static readonly Dictionary<int, GameplayTag> GameplayTags = new();

        private static AbilitySystemManager _instance;

        private static uint _attributeModifierId;
        public static uint GetNextModifierId() => ++_attributeModifierId;

        public static AbilitySystemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<AbilitySystemManager>();
                    if (_instance != null) return _instance;
                }

                var go = new GameObject("[AbilitySystem] GameplayTagsManager");
                _instance = go.AddComponent<AbilitySystemManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                Initialize();
            }
        }

        private void OnDestroy()
        {
            if (_instance != this) return;
            
            _instance = null;
            _attributeModifierId = 0;
        }

        private static void Initialize()
        {
            GameplayTags.Clear();
            
            var loadedCustomTags = Resources.LoadAll<GameplayTagConfig>("AbilitySystem/CustomTags");
            foreach (var customTag in loadedCustomTags)
            {
                GameplayTags.Add(customTag.TagInstance.Id, customTag.TagInstance);
            }
        }
    }
}
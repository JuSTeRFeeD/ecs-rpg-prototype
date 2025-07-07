using System;
using Heavenage.Scripts.ECS.Runtime.Views;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.Authoring.Characters
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct CharacterAuthoringComponent : IComponent
    {
        public Transform spawnPoint;
        public EntityView viewPrefab;

        private GameObject _authoringObject;
        public void SetAuthoringObject(GameObject obj) => _authoringObject = obj;
        public GameObject GetAuthoringObject() => _authoringObject;
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class CharacterAuthoring : MonoProvider<CharacterAuthoringComponent>
    {
        protected override void OnValidate()
        {
            GetSerializedData().spawnPoint = transform;
            GetSerializedData().SetAuthoringObject(gameObject);
        }
    }
}
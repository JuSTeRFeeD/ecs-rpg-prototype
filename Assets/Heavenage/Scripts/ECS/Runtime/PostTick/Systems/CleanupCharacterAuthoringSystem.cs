using Heavenage.Scripts.ECS.Runtime.Authoring.Characters;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.PostTick.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CleanupCharacterAuthoringSystem : ISystem
    {
        [Inject] private Stash<CharacterAuthoringComponent> _characterAuthoringStash;
        
        public World World { get; set; }

        private Filter _filter;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterAuthoringComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                Object.Destroy(_characterAuthoringStash.Get(entity).GetAuthoringObject());
                World.RemoveEntity(entity);
            }
        }

        public void Dispose()
        {
        }
    }
}
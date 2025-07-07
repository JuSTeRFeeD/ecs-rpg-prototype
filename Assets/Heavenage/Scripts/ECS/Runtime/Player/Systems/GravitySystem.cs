using Heavenage.Scripts.ECS.Runtime.Player.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Player.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GravitySystem : ISystem
    {
        [Inject] private Stash<CharacterControllerReference> _characterControllerReferenceStash;
        [Inject] private Stash<CharacterMoveDataComponent> _characterMoveDataComponentStash;
        
        public World World { get; set; }

        private Filter _filter;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CharacterControllerReference>()
                .With<CharacterMoveDataComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterControllerReference = ref _characterControllerReferenceStash.Get(entity);
                ref var moveData = ref _characterMoveDataComponentStash.Get(entity);
                var characterController = characterControllerReference.Value;
                
                if (characterController.isGrounded && moveData.YVelocity < 0)
                    moveData.YVelocity = 0;
                else
                    moveData.YVelocity += moveData.Gravity * deltaTime;
                
                characterController.Move(new Vector3(0f, moveData.YVelocity, 0f) * deltaTime);
            }
        }

        public void Dispose()
        {
        }
    }
}
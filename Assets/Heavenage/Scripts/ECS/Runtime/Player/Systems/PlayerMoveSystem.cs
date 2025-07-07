using Heavenage.Scripts.CameraControls;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components;
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
    public sealed class PlayerMoveSystem : ISystem
    {
        [Inject] private Stash<CharacterControllerReference> _characterControllerReferenceStash;
        [Inject] private Stash<CharacterMoveDataComponent> _characterMoveDataComponentStash;
        [Inject] private Stash<CameraControllerReference> _controlledCameraReferenceStash;
        [Inject] private Stash<PlayerMoveInputComponent> _playerMoveInputComponentStash;
        [Inject] private Stash<AttributeComponent> _attributeComponentStash;
        
        public World World { get; set; }

        private Filter _characterFilter;
        private Filter _cameraFilter;
        
        private const string MoveSpeedAttribute = "MoveSpeed";
        private static readonly int MoveSpeedAttributeHash = MoveSpeedAttribute.GetHashCode();

        public void OnAwake()
        {
            _characterFilter = World.Filter
                .With<PlayerMoveInputComponent>()
                .With<CharacterControllerReference>()
                .With<AttributeComponent>()
                .Build();

            _cameraFilter = World.Filter
                .With<CameraControllerReference>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            CameraController cameraController = null;
            foreach (var entity in _cameraFilter)
            {
                cameraController = _controlledCameraReferenceStash.Get(entity).CameraController;
                break;
            }
            if (!cameraController) return;

            foreach (var entity in _characterFilter)
            {
                ref var characterControllerReference = ref _characterControllerReferenceStash.Get(entity);
                ref var characterMoveDataComponent = ref _characterMoveDataComponentStash.Get(entity);
                ref var playerMoveInputComponent = ref _playerMoveInputComponentStash.Get(entity);
                ref var attributeComponent = ref _attributeComponentStash.Get(entity);
                
                if (playerMoveInputComponent.MoveInput.magnitude < 0.1f) 
                    continue;
                
                var input = playerMoveInputComponent.MoveInput;
                var cameraTransform = cameraController.MainCameraTransform;
                var characterController = characterControllerReference.Value;
                
                // Rotate
                var targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(
                    characterController.transform.eulerAngles.y, 
                    targetAngle,
                    ref characterMoveDataComponent.RotationSpeed, 
                    0.1f);
                characterController.transform.rotation = Quaternion.Euler(0, angle, 0);

                // Move
                var moveSpeed = attributeComponent.AttributeMap[MoveSpeedAttributeHash].FinalValue;
                var moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDirection.normalized * (moveSpeed * deltaTime));
            }
        }

        public void Dispose()
        {
        }
    }
}
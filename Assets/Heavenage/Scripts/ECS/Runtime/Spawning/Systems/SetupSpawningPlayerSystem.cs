using Heavenage.Scripts.CameraControls;
using Heavenage.Scripts.ECS.Runtime.Authoring.Characters;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Player.Components;
using Heavenage.Scripts.ECS.Runtime.Spawning.Components;
using Heavenage.Scripts.ECS.Runtime.Views;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Spawning.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SetupSpawningPlayerSystem : ISystem
    {
        [Inject] private CameraController _cameraController;
        
        [Inject] private Stash<PlayerTag> _playerTagStash;
        [Inject] private Stash<EntityViewComponent> _entityViewStash;
        [Inject] private Stash<PlayerMoveInputComponent> _playerMoveInputStash;
        [Inject] private Stash<CharacterControllerReference> _characterControllerReferenceStash;
        [Inject] private Stash<CharacterMoveDataComponent> _characterMoveDataComponentStash;
        [Inject] private Stash<SpawningEntityComponent> _spawningEntityStash;
        
        [Inject] private Stash<CameraControllerReference> _cameraControllerReferenceStash;

        // TODO: Move to config
        private const float DefaultGravity = -9.81f;
        
        public World World { get; set; }

        private Filter _filter;
        private Filter _cameraFilter;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<SpawningEntityComponent>()
                .With<IsPlayerAuthoringComponent>()
                .Build();

            _cameraFilter = World.Filter
                .With<CameraControllerReference>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var spawningEntity = ref _spawningEntityStash.Get(entity).Entity;

                _playerTagStash.Add(spawningEntity);
                
                // View controller
                var view = _entityViewStash.Get(spawningEntity).Value as PlayerView;
                _cameraControllerReferenceStash.Get(_cameraFilter.First()).CameraController.SetTarget(view.CameraTarget);

                // Movement
                _playerMoveInputStash.Add(spawningEntity, new PlayerMoveInputComponent());
                _characterControllerReferenceStash.Add(spawningEntity, new CharacterControllerReference
                {
                    Value = view.GetComponent<CharacterController>()
                });
                _characterMoveDataComponentStash.Add(spawningEntity, new CharacterMoveDataComponent
                {
                    Velocity = Vector3.zero,
                    YVelocity = 0f,
                    Gravity = DefaultGravity,
                    RotationSpeed = 10f,
                });
            }
        }

        public void Dispose()
        {
        }
    }
}
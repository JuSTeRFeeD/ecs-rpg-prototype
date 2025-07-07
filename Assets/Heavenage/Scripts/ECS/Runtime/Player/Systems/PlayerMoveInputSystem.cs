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
    public sealed class PlayerMoveInputSystem : ISystem
    {
        [Inject] private Stash<PlayerMoveInputComponent> _playerMoveInputStash;
        
        public World World { get; set; }

        private Filter _filter;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerMoveInputComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var moveInput = ref _playerMoveInputStash.Get(entity);
                moveInput.MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                moveInput.Jump = Input.GetKeyDown(KeyCode.Space);
            }
        }

        public void Dispose()
        {
        }
    }
}
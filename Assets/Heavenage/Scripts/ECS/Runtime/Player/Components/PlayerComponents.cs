using Heavenage.Scripts.CameraControls;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.Player.Components
{
    public struct PlayerTag : IComponent { }

    public struct CharacterControllerReference : IComponent
    {
        public CharacterController Value;
    }

    public struct CharacterMoveDataComponent : IComponent
    {
        public Vector3 Velocity;
        public float YVelocity;
        public float Gravity;
        public float RotationSpeed;
    }

    public struct PlayerMoveInputComponent : IComponent
    {
        public Vector2 MoveInput;
        public bool Jump;
    }

    public struct CameraControllerReference : IComponent
    {
        public CameraController CameraController;
    }
}
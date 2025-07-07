using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.Views
{
    public class PlayerView : EntityView
    {
        [field: SerializeField] public Transform CameraTarget { get; private set; }
    }
}
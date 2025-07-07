using Heavenage.Scripts.CameraControls;
using Heavenage.Scripts.ECS.Runtime.Player.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Heavenage.Scripts.ECS.Runtime.Player.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CameraControllerInitializer : IInitializer
    {
        [Inject] private CameraController _cameraController;
        [Inject] private Stash<CameraControllerReference> _cameraControllerReferenceStash;
        
        public World World { get; set; }

        public void OnAwake()
        {
            var entity = World.CreateEntity();
            _cameraControllerReferenceStash.Add(entity, new CameraControllerReference
            {
                CameraController = _cameraController
            });
        }

        public void Dispose()
        {
        }
    }
}
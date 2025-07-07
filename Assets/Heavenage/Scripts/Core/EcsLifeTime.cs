using Heavenage.Scripts.CameraControls;
using Heavenage.Scripts.ECS.Runtime;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Heavenage.Scripts.Core
{
    public class EcsLifeTime : ExtendedLifetime
    {
        [SerializeField] private CameraController cameraController;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<CameraController>(cameraController);
            
            // Setup and Register ECS
            builder.RegisterInstance<World>(World.Default);
            StashRegistry.RegisterStashes(builder, World.Default);
            builder.RegisterEntryPoint<EcsBootstrapper>();
        }
    }
}

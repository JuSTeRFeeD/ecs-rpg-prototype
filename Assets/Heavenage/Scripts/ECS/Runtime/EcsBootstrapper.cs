using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Systems;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Damage.Systems;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Projectiles.Systems;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Systems;
using Heavenage.Scripts.ECS.Runtime.Common;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Heavenage.Scripts.ECS.Runtime.Player.Systems;
using Heavenage.Scripts.ECS.Runtime.PostTick.Systems;
using Heavenage.Scripts.ECS.Runtime.Spawning.Systems;
using Scellecs.Morpeh;
using VContainer;
using VContainer.Unity;

namespace Heavenage.Scripts.ECS.Runtime
{
    public class EcsBootstrapper : IInitializable
    {
        private readonly World _world;
        
        [Inject]
        public EcsBootstrapper(IObjectResolver resolver, World world)
        {
            _world = world;
            SystemGroupExtension.SetResolver(resolver);
        }
        
        public void Initialize()
        {
            _world.UpdateByUnity = true;
            
            AddInitializers(0);
            AddSpawnSystems(1);
            AddPlayerSystems(2);
            AddAbilitySystems(3);
            AddProjectileSystems(4);

            AddPostTickSystems(100);
        }

        private void AddInitializers(int order)
        {
            var systemGroup = _world.CreateSystemsGroup();
            
            systemGroup.AddInitializer<ViewDisposableInitializer>();
            systemGroup.AddInitializer<CameraControllerInitializer>();
            
            _world.AddSystemsGroup(order, systemGroup);
        }

        private void AddSpawnSystems(int order)
        {
            var systemGroup = _world.CreateSystemsGroup();
            
            systemGroup.AddSystem<SpawnCharacterSystem>();
            systemGroup.AddSystem<SetupSpawningPlayerSystem>();
            systemGroup.AddSystem<SetupSpawningAttributesSystem>();
            systemGroup.AddSystem<SetupSpawningAbilitiesSystem>();
            
            _world.AddSystemsGroup(order, systemGroup);
        }

        private void AddPlayerSystems(int order)
        {
            var systemGroup = _world.CreateSystemsGroup();
            
            systemGroup.AddSystem<PlayerUseAbilitySystem>();
            
            systemGroup.AddSystem<PlayerMoveInputSystem>();
            systemGroup.AddSystem<PlayerMoveSystem>();
            systemGroup.AddSystem<GravitySystem>();
            
            _world.AddSystemsGroup(order, systemGroup);
        }

        private void AddAbilitySystems(int order)
        {
            var systemGroup = _world.CreateSystemsGroup();
            
            // === Abilities ===
            
            // Activation
            systemGroup.AddSystem<ActivateAbilitySystem>();
            systemGroup.AddSystem<SetAbilityInputReleasedSystem>();
            
            // Progress
            systemGroup.AddSystem<AbilityExecutionSystem>();
            
            // === Attributes ===
            systemGroup.AddSystem<AttributeModifierSystem>();
            
            // === Damage ===
            systemGroup.AddSystem<DoTDamageSystem>();
            
            _world.AddSystemsGroup(order, systemGroup);
        }

        private void AddProjectileSystems(int order)
        {
            var systemGroup = _world.CreateSystemsGroup();
            
            systemGroup.AddSystem<ProjectileMovementSystem>();
            systemGroup.AddSystem<ProjectileHitAbilitySystem>();
            
            _world.AddSystemsGroup(order, systemGroup);
        }

        private void AddPostTickSystems(int order)
        {
            var systemGroup = _world.CreateSystemsGroup();
            
            systemGroup.AddSystem<CleanupCharacterAuthoringSystem>();
            
            systemGroup.AddSystem<DestroyOverTimeSystem>();
            systemGroup.AddSystem<DestroySystem>();
            
            _world.AddSystemsGroup(order, systemGroup);
        }
    }
}
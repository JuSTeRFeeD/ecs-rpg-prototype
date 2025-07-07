using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.MovementLogic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Projectiles.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Heavenage.Scripts.ECS.Runtime.Utils;
using Heavenage.Scripts.ECS.Runtime.Views;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles
{
    [CreateAssetMenu(menuName = "RPG/SubAbility/Projectile/SpawnProjectileSubAbility")]
    public class SpawnProjectileSubAbility : SubAbilityTask<SpawnProjectileAbilityTask>
    {
        public EntityView projectilePrefab;
        public float speed;
        [Tooltip("Projectile will be destroyed after this time")]
        public float lifetime = float.MaxValue;
        public bool destroyOnHit = true;
        
        [Space]
        [Tooltip("Create or reuse projectile movement logic")]
        [InlineEditor]
        public ProjectileMovementDefinition movementLogic;
        
        [Space]
        [Tooltip("[Optional] Apply other ability on hit")]
        [InlineEditor]
        public AbilityDefinition onHitAbility;
    }

    public class SpawnProjectileAbilityTask : IAbilityTaskWithSetup
    {
        private EntityView _projectilePrefab;
        private float _speed;
        private float _lifetime;
        private AbilityDefinition _onHitAbility;
        private ProjectileMovementDefinition _movementDefinition;
        private bool _destroyOnHit;
        
        public void SetupFromSubAbility(SubAbility ability)
        {
            var data = (SpawnProjectileSubAbility)ability;
            _projectilePrefab = data.projectilePrefab;
            _speed = data.speed;
            _lifetime = data.lifetime;
            _onHitAbility = data.onHitAbility;
            _movementDefinition = data.movementLogic;
            _destroyOnHit = data.destroyOnHit;
        }

        public bool Tick(Entity caster, Entity target, World world, float deltaTime)
        {
            var projectileEntity = world.CreateEntity();

            var view = StashRegistry.GetStash<EntityViewComponent>().Get(caster).Value;
            // TODO: set spawn position & rotation by logic 
            projectileEntity.InstantiateView(_projectilePrefab, view.transform.position, view.transform.rotation);
            
            StashRegistry.GetStash<ProjectileComponent>().Set(projectileEntity, new ProjectileComponent
            {
                Direction = view.transform.forward, // TODO: set direction by logic
                Source = caster,
                Speed = _speed,
                OnHitAbility = _onHitAbility,
                DestroyOnHit = _destroyOnHit
            });
            
            StashRegistry.GetStash<DestroyOverTimeComponent>().Set(projectileEntity, new DestroyOverTimeComponent
            {
                EstimateTime = _lifetime
            });
            
            // Set movement logic
            var moveProjectileEntity = world.CreateEntity();
            var logic = _movementDefinition.CreateLogic();
            logic.Initialize(projectileEntity, caster, target, world);
            StashRegistry.GetStash<ActiveProjectileComponent>().Set(moveProjectileEntity, new ActiveProjectileComponent
            {
                Projectile = projectileEntity,
                Logic = logic
            });
            
            LinkedEntityUtils.Link(projectileEntity, moveProjectileEntity);
            
            return true;
        }
    }
}
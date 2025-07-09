using System;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.MovementLogic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.SpawnStrategy;
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
        [Tooltip("Spawn projectile in world or towards target")]
        [Required]
        public ProjectileSpawnStrategy spawnStrategy;
        
        [Tooltip("Create or reuse projectile movement logic")]
        [InlineEditor]
        [Required]
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
        private bool _destroyOnHit;
        private AbilityDefinition _onHitAbility;
        private ProjectileMovementDefinition _movementDefinition;
        private IProjectileSpawnStrategy _projectileSpawnStrategy;
        
        public void SetupFromSubAbility(SubAbility ability)
        {
            var data = (SpawnProjectileSubAbility)ability;
            _projectilePrefab = data.projectilePrefab;
            _speed = data.speed;
            _lifetime = data.lifetime;
            _onHitAbility = data.onHitAbility;
            _movementDefinition = data.movementLogic;
            _destroyOnHit = data.destroyOnHit;
            _projectileSpawnStrategy = data.spawnStrategy;
        }

        public bool Tick(Entity activeAbility, Entity caster, Entity target, World world, float deltaTime)
        {
            var casterView = StashRegistry.GetStash<EntityViewComponent>().Get(caster).Value;
            var originPos = casterView.transform.position;
            
            foreach (var (direction, targetEntity) in _projectileSpawnStrategy.Resolve(activeAbility, caster, world))
            {
                Debug.Log("Spawned");
                SpawnProjectile(originPos,  direction, caster, targetEntity, world);
            }

            return true;
        }

        private void SpawnProjectile(Vector3 origin, Vector3 direction, Entity caster, Entity? target, World world)
        {
            var projectileEntity = world.CreateEntity();

            projectileEntity.InstantiateView(_projectilePrefab, origin, Quaternion.LookRotation(direction));
            
            StashRegistry.GetStash<ProjectileComponent>().Set(projectileEntity, new ProjectileComponent
            {
                Direction = direction,
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
            var moveEntity = world.CreateEntity();
            var logic = _movementDefinition.CreateLogic();
            logic.Initialize(projectileEntity, caster, target, world);
            StashRegistry.GetStash<ActiveProjectileComponent>().Set(moveEntity, new ActiveProjectileComponent
            {
                Projectile = projectileEntity,
                Logic = logic
            });
            
            LinkedEntityUtils.Link(projectileEntity, moveEntity);
        }
    }
}
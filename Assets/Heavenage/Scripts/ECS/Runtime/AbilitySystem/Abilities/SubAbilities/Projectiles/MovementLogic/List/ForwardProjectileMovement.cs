using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Projectiles.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Heavenage.Scripts.ECS.Runtime.Views;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.SubAbilities.Projectiles.MovementLogic.List
{
    [CreateAssetMenu(menuName = "RPG/SubAbility/ProjectileMovement/ForwardProjectileMovement")]
    public class ForwardProjectileMovement : ProjectileMovementDefinition
    {
        public override IProjectileMovementLogic CreateLogic()
        {
            return new ForwardLogic();
        }

        private class ForwardLogic : IProjectileMovementLogic
        {
            private float _speed;
            private Vector3 _direction;
            
            public void Initialize(Entity projectile, Entity source, Entity? target, World world)
            {
                ref readonly var projectileData = ref StashRegistry.GetStash<ProjectileComponent>().Get(projectile); 
                _speed = projectileData.Speed;
                _direction = projectileData.Direction;
            }

            public void Tick(Entity projectile, World world, float deltaTime)
            {
                var view = StashRegistry.GetStash<EntityViewComponent>().Get(projectile).Value;

                var nextPos = view.transform.position + _direction * (_speed * deltaTime);

                if (Physics.Raycast(view.transform.position, _direction, out var hit, _speed * deltaTime))
                {
                    var projectileComponent = StashRegistry.GetStash<ProjectileComponent>().Get(projectile);
                    
                    if (projectileComponent.OnHitAbility)
                    {
                        if (hit.collider.TryGetComponent(out EntityView entityView))
                        {
                            StashRegistry.GetStash<ProjectileHitComponent>().Add(projectile, new ProjectileHitComponent
                            {
                                HitEntity = entityView.Entity
                            });
                        }
                    }

                    if (projectileComponent.DestroyOnHit)
                    {
                        StashRegistry.GetStash<ToDestroyTag>().Set(projectile, new ToDestroyTag());
                    }
                }
                else
                {
                    view.transform.position = nextPos;
                }
            }
        }
    }
}
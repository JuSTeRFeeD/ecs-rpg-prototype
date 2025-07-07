using System.Collections.Generic;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes.Components;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Heavenage.Scripts.ECS.Runtime.Utils;
using Heavenage.Scripts.ECS.Runtime.Views;
using Heavenage.Scripts.ECS.Runtime.Views.AbilityIndicators;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting.List
{
    public struct CircleAreaTargetingComponent : IComponent
    {
        public float BaseRadius;
        public float CurrentRadius;
        public Vector3 Origin;
        public Entity Indicator;
    }

    [CreateAssetMenu(menuName = "RPG/SubAbility/Targeting/CircleArea")]
    public class CircleAreaTargetingStrategy : TargetingStrategy
    {
        [MinValue(0.5f)] public float radius = 0.5f;

        public override ITargetingStrategy CreateTargetingStrategy(EntityView indicatorView)
        {
            return new CircleAreaStrategy(radius, indicatorView);
        }
    }

    public class CircleAreaStrategy : ITargetingStrategy
    {
        private readonly float _radius;
        private readonly CircleIndicatorView _circleIndicatorViewPrefab;
        
        private Stash<CircleAreaTargetingComponent> _circleAreaTargetingStash;
        private Stash<EntityViewComponent> _viewStash;

        private bool _isInitialized;
        private Entity _caster;

        public CircleAreaStrategy(float radius, EntityView indicatorView)
        {
            _circleIndicatorViewPrefab = (CircleIndicatorView)indicatorView;
            _radius = radius;
        }

        public void OnStart(Entity caster, World world)
        {
            if (_isInitialized) return;
            _isInitialized = true;

            _caster = caster;
            
            _viewStash = StashRegistry.GetStash<EntityViewComponent>();
            _circleAreaTargetingStash = StashRegistry.GetStash<CircleAreaTargetingComponent>();

            var indicatorEntity = world.CreateEntity();
            var spawnPos = _viewStash.Get(caster).Value.transform.position;
            indicatorEntity.InstantiateView(_circleIndicatorViewPrefab, spawnPos, Quaternion.identity);
            LinkedEntityUtils.Link(caster, indicatorEntity);
            
            _circleAreaTargetingStash.Set(caster, new CircleAreaTargetingComponent
            {
                BaseRadius = _radius,
                CurrentRadius = _radius,
                Origin = _viewStash.Get(caster).Value.transform.position,
                Indicator = indicatorEntity
            });
            
        }

        public void Tick(Entity caster, World world)
        {
            _circleAreaTargetingStash.Get(caster).Origin = _viewStash.Get(caster).Value.transform.position;
            Debug.Log("Origin: " + _circleAreaTargetingStash.Get(caster).Origin);
        }

        public void OnEnd(Entity caster, World world)
        {
            world.RemoveEntity(_circleAreaTargetingStash.Get(caster).Indicator);
            _circleAreaTargetingStash.Remove(caster);
        }

        public IEnumerable<Entity> GetTargets(Entity caster, World world)
        {
            var circleAreaTargeting = _circleAreaTargetingStash.Get(caster);
            var attributeStash = world.GetStash<AttributeComponent>();
            var result = new List<Entity>();

            var hits = Physics.OverlapSphere(circleAreaTargeting.Origin, circleAreaTargeting.CurrentRadius);
            foreach (var hit in hits)
            {
                if (!hit.TryGetComponent(out EntityView view)) continue;
                if (attributeStash.Has(view.Entity))
                {
                    result.Add(view.Entity);
                }
            }

            return result;
        }

        public void SetRadius(float newRadius)
        {
            _circleAreaTargetingStash.Get(_caster).CurrentRadius = newRadius;
        }
    }
}
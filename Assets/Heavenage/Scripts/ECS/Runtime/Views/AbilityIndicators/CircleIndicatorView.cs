using System;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities.Targeting.List;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Heavenage.Scripts.ECS.Runtime.Views.AbilityIndicators
{
    public class CircleIndicatorView : EntityView
    {
        [SerializeField] private DecalProjector decalProjector;
        
        private Stash<CircleAreaTargetingComponent> _circleAreaTargetingStash;
        private Stash<OwnerComponent> _ownerStash;

        private void OnEnable()
        {
            var size = new Vector3(0f, 0f, 0f);
            decalProjector.size = size;
        }

        protected override void OnInit()
        {
            _circleAreaTargetingStash = StashRegistry.GetStash<CircleAreaTargetingComponent>();
            _ownerStash = StashRegistry.GetStash<OwnerComponent>();
            
            // ref readonly var owner = ref _ownerStash.Get(Entity).Value;
            // ref readonly var targeting = ref _circleAreaTargetingStash.Get(owner);
            // var targetPos = targeting.Origin + Vector3.up * 2f;
            // transform.position = targetPos;
        }

        private void Update()
        {
            if (!IsInit) return;
            
            ref readonly var owner = ref _ownerStash.Get(Entity).Value;
            ref readonly var targeting = ref _circleAreaTargetingStash.Get(owner);

            var targetPos = targeting.Origin + Vector3.up * 2f;
            transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * 50f);

            var size = new Vector3(targeting.CurrentRadius, targeting.CurrentRadius, 5f);
            decalProjector.size = size;
        }
    }
}
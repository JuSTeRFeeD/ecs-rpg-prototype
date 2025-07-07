using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Heavenage.Scripts.ECS.Runtime.Views;
using Heavenage.Scripts.MyPooling;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.Utils
{
    public static class SpawnUtils
    {
        public static EntityView InstantiateView(
            this Entity entity, 
            EntityView viewPrefab, 
            Vector3 position = default,
            Quaternion rotation = default)
        {
            var spawned = MyPool.Instance.Spawn(viewPrefab, position, rotation);
            spawned.Init(entity);
            StashRegistry.GetStash<EntityViewComponent>().Set(entity, new EntityViewComponent
            {
                Value = spawned
            });
            return spawned;
        }
    }
}
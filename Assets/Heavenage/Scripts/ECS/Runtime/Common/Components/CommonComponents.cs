using System;
using Heavenage.Scripts.ECS.Runtime.Views;
using Heavenage.Scripts.MyPooling;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace Heavenage.Scripts.ECS.Runtime.Common.Components
{
    public struct ToDestroyTag : IComponent { }

    public struct DestroyOverTimeComponent : IComponent
    {
        public float EstimateTime;
    }
    
    public struct EntityViewComponent : IComponent, IDisposable
    {
        public EntityView Value;
        
        public void Dispose()
        {
#if UNITY_EDITOR
            if (Value) MyPool.Instance.Despawn(Value.gameObject);
#else
            MyPool.Instance.Despawn(Value.gameObject);
 #endif
        }
    }

    public struct OwnerComponent : IComponent
    {
        public Entity Value;
    }

    public struct LinkedEntitiesComponent : IComponent
    {
        public FastList<Entity> Values;
    }
}
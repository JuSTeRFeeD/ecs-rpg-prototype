using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Runtime.Views
{
    public class EntityView : MonoBehaviour
    {
        public Entity Entity { get; private set;}
        protected bool IsInit { get; private set; }
        
        public void Init(Entity owner)
        {
            Entity = owner;
            IsInit = true;
            OnInit();
        }

        private void OnDisable()
        {
            IsInit = false;
        }

        protected virtual void OnInit() { }
    }
}
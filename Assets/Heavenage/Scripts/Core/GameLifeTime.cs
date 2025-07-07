using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.Core
{
    public class GameLifeTime : ExtendedLifetime
    {
        [SerializeField] private CanvasGroup loadingCanvasGroup;
        
        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(gameObject);
            
            var coroutineRunnerGo = new GameObject();
            var coroutineRunner = coroutineRunnerGo.AddComponent<CoroutineRunner>();
            builder.RegisterInstance(coroutineRunner).As<ICoroutineRunner>();
            
            builder.RegisterInstance(new SceneLoader(this, loadingCanvasGroup, coroutineRunner));
        }
    }
}

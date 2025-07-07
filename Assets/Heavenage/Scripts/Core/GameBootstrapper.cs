using UnityEngine;
using VContainer;

namespace Heavenage.Scripts.Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;

        private void Start()
        {
            _sceneLoader.LoadSceneAsync("Game");
        }
    }
}
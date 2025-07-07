using System.Collections;
using UnityEngine;

namespace Heavenage.Scripts.Core
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator enumerator);
        public void StopCoroutine(IEnumerator coroutine);
        public void StopAllCoroutines();
    }
    
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
    }
}
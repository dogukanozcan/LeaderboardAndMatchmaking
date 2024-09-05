using System;
using System.Collections;
using UnityEngine;

namespace Naku.CoreSystem
{
    public class MonoSingletonDelay<T> : MonoSingleton<T> where T : MonoSingletonDelay<T>
    {
        public void Delay(Action action, float delay)
        {
            StartCoroutine(Delay_Handler(action, delay));
        }
        private IEnumerator Delay_Handler(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();

        }
    }
}
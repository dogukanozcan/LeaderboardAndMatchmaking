using System.Collections.Generic;
using UnityEngine;

namespace Naku.CoreSystem
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static volatile T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType(typeof(T)) as T;
                }

                return instance;
            }
        }
        public void OnEnable()
        {
            if (instance == null)
                instance = FindAnyObjectByType(typeof(T)) as T;

            if (instance != null && instance != this)
                Destroy(gameObject);
        }
    }
}
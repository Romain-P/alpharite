using System;
using StunShared;
using UnityEngine;

namespace AlphaRite
{
    public class AlphaRiteLoader {
        private static GameObject _alphaRiteInstance;
        
        public static void init() {
            Alpharite.println("Initializing injection");
            _alphaRiteInstance = new GameObject();
                
            _alphaRiteInstance.transform.parent = null;
            var gameRootObject = _alphaRiteInstance.transform.root;
            if (gameRootObject != null && gameRootObject.gameObject != _alphaRiteInstance)
                gameRootObject.parent = _alphaRiteInstance.transform;

            _alphaRiteInstance.AddComponent<AlphaScriptAdapter>();
            GameObject.DontDestroyOnLoad(_alphaRiteInstance);
            Alpharite.println("Injection success");
        }

        public static void stop() {
            Alpharite.println("Stopping Alpharite");
            GameObject.Destroy(_alphaRiteInstance);
            Alpharite.println("Alpharite stopped");
        }
    }
}

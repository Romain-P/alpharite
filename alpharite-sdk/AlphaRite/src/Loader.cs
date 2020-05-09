using UnityEngine;

namespace AlphaRite
{
    public class AlphaRiteLoader {
        private static GameObject _alphaRiteInstance;
        
        public static void init() {
            _alphaRiteInstance = new GameObject();
                
            _alphaRiteInstance.transform.parent = null;
            var gameRootObject = _alphaRiteInstance.transform.root;
            if (gameRootObject != null && gameRootObject.gameObject != _alphaRiteInstance)
                gameRootObject.parent = _alphaRiteInstance.transform;

            _alphaRiteInstance.AddComponent<AlphaScriptAdapter>();
            GameObject.DontDestroyOnLoad(_alphaRiteInstance);
        }

        public static void stop() {
            GameObject.Destroy(_alphaRiteInstance);
        }
    }
}

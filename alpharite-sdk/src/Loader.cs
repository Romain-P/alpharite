using UnityEngine;

namespace AlphaRite
{
    public class AlphaRiteLoader: MonoBehaviour {
        private static GameObject _alphaRiteInstance;
        
        public static void init() {
            _alphaRiteInstance = new GameObject();
            _alphaRiteInstance.AddComponent<ScriptAdapter>();

            GameObject.DontDestroyOnLoad(_alphaRiteInstance);
        }

        public static void stop() {
            GameObject.Destroy(_alphaRiteInstance);
        }
    }
}

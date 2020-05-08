using AlphaRite.sdk;
using UnityEngine;

namespace AlphaRite
{
    /**
     * This class is acting like a loader
     * Must be injected with an injector
     * 
     * EntryPoint: Main.main
     * DestroyPoint: Main.destroy
     */
    public class Main: MonoBehaviour {
        private static GameObject _alphaRiteInstance;
        private AlphariteSdk _sdk;

        public void Start() {
            _sdk = new AlphariteSdk();
        }

        public void Update() {
            _sdk.onUpdate();
        }

        public void OnGUI() {
            _sdk.onRenderingUpdate();
        }

        public void OnDisable() {
            _sdk.onStop();
        }

        public static void main() {
            Main._alphaRiteInstance = new GameObject();
            Main._alphaRiteInstance.AddComponent<Main>();

            GameObject.DontDestroyOnLoad(Main._alphaRiteInstance);
        }

        public static void destroy() {
            GameObject.Destroy(Main._alphaRiteInstance);
        }
    }
}

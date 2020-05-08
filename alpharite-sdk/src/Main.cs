using AlphaRite.Sdk;
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
        private AlphariteSdk sdk;

        public void Start() {
            this.sdk = new AlphariteSdk();
        }

        public void Update() {
            this.sdk.onUpdate();
        }

        public void OnGUI() {
            this.sdk.onRenderingUpdate();
        }

        public void OnDisable() {
            this.sdk.onStop();
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

using AlphaRite.sdk;
using UnityEngine;

namespace AlphaRite {
    public class ScriptAdapter: MonoBehaviour{
        private AlphariteSdk _sdk;
        
        public void Start() {
            _sdk = new AlphariteSdk();
            _sdk.onStart();
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
    }
}
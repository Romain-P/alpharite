using AlphaRite.sdk;
using UnityEngine;

namespace AlphaRite {
    public class ScriptAdapter: MonoBehaviour{
        private AlphariteSdk _sdk;
        
        public void Start() {
            _sdk = new AlphariteSdk();
            _sdk.enable();
        }

        public void Update() {
            _sdk.update();
        }

        public void OnGUI() {
            _sdk.renderingUpdate();
        }

        public void OnDisable() {
            _sdk.disable();
        }
    }
}
﻿using UnityEngine;

namespace AlphaRite {
    using sdk;
    
    public class AlphaScriptAdapter: MonoBehaviour{
        private AlphariteSdk _sdk;

        public AlphaScriptAdapter() {
            _sdk = new AlphariteSdk();
        }
        
        public void Start() {
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
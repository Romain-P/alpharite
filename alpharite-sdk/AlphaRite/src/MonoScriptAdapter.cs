﻿using UnityEngine;

namespace AlphaRite {
    using sdk;
    
    public class MonoScriptAdapter: MonoBehaviour{
        private AlphariteSdk _sdk;

        public MonoScriptAdapter() {
            _sdk = new AlphariteSdk();
        }
        
        public void Start() {
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
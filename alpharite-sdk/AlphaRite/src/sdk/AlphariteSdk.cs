﻿using System.Collections.Generic;
using AlphaRite.sdk.hacks;
 using HarmonyLib;

 namespace AlphaRite.sdk {
    public class AlphariteSdk {
        private List<AlphaCycle> _cycles;
        public ReferenceHolder refs { get; }
        public Harmony patcher { get; }
        
        public AlphariteSdk() {
            patcher = new Harmony("AlphaRite.sdk");
            
            _cycles = new List<AlphaCycle>();
            refs = new ReferenceHolder();
            
            subscribeHacks();
            subscribeScripts();
        }

        void subscribeHacks() {
            _cycles.Add(new WallHack(this));
        }

        void subscribeScripts() {
            // TODO
        }

        public void onStart() {
            foreach (var cycle in _cycles)
                cycle.enable();
        }

        public void onStop() {
            foreach (var cycle in _cycles)
                cycle.disable();
        }

        public void onRenderingUpdate() {
            foreach (var cycle in _cycles)
                cycle.renderingUpdate();
        }

        public void onUpdate() {
            foreach (var cycle in _cycles)
                cycle.update();
        }
    }
}
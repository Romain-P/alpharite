﻿using System.Collections.Generic;
using AlphaRite.sdk.hacks;

namespace AlphaRite.sdk {
    public class AlphariteSdk : AlphaCycle {
        private List<AlphaCycle> _cycles;
        public ReferenceHolder references { get; }
        
        public AlphariteSdk() : base(null) {
            _cycles = new List<AlphaCycle>();
            references = new ReferenceHolder();
            
            subscribeHacks();
            subscribeScripts();
        }

        void subscribeHacks() {
            _cycles.Add(new WallHack(this));
        }

        void subscribeScripts() {
            // TODO
        }

        protected override void onStart() {
            foreach (var cycle in _cycles)
                cycle.enable();
        }

        protected override void onStop() {
            foreach (var cycle in _cycles)
                cycle.disable();
        }

        protected override void onRenderingUpdate() {
            foreach (var cycle in _cycles)
                cycle.renderingUpdate();
        }

        protected override void onUpdate() {
            foreach (var cycle in _cycles)
                cycle.update();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using AlphaRite.sdk;
using UnityEngine;

namespace AlphaRite {
    public class InputHandler {
        private Queue<ManagedInput> _inputs;

        public InputHandler() {
            _inputs = new Queue<ManagedInput>();
        }

        public void onUpdate() {
            if (_inputs.Count == 0) return;
            var next = _inputs.First();

            if (next.modifierCode != -1 && !next.modifierApplied) {
                UnmanagedUtil.keyDown(next.modifierCode);
                next.modifierApplied = true;
                return;
            }

            switch (next.type) {
                case ManagedInputType.Keyboard:
                    UnmanagedUtil.keyPressed(next.code);
                    break;
                case ManagedInputType.MouseClick:
                    UnmanagedUtil.mousePressed(next.code);
                    break;
                case ManagedInputType.MouseWheel:
                    UnmanagedUtil.mouseWheelDown();
                    break;
            }
            
            if (next.modifierApplied)
                UnmanagedUtil.keyUp(next.modifierCode);
            _inputs.Dequeue();
        }

        public void add(KeyCode code, KeyCode? modifier = null) {
            var isMouse = code.ToString().ToLower().Contains("mouse");
            var type = isMouse ? ManagedInputType.MouseClick : ManagedInputType.Keyboard;
            var modifierCode = modifier != null ? VirtualKeys.mappedKeys[(KeyCode) modifier] : -1;

            _inputs.Enqueue(new ManagedInput(type, VirtualKeys.mappedKeys[code], modifierCode));
        }

        public void addMousewheel(KeyCode? modifier = null) {
            var type = ManagedInputType.MouseWheel;
            var modifierCode = modifier != null ? VirtualKeys.mappedKeys[(KeyCode) modifier] : -1;
            _inputs.Enqueue(new ManagedInput(type, 0x0, modifierCode));
        }
    }

    public enum ManagedInputType {
        Keyboard,
        MouseClick,
        MouseWheel
    }
    
    public struct ManagedInput {
        public ManagedInputType type;
        public int code;
        public bool modifierApplied;
        public int modifierCode;

        public ManagedInput(ManagedInputType type, int code, int modifierCode = -1) {
            this.type = type;
            this.code = code;
            this.modifierCode = modifierCode;
            this.modifierApplied = false;
        }
    }
}
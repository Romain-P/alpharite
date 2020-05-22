using System.Collections.Generic;
using System.Linq;
using AlphaRite.sdk;
using UnityEngine;

namespace AlphaRite {
    public class InputHandler {
        private readonly Queue<ManagedInput> _inputs;

        public InputHandler() {
            _inputs = new Queue<ManagedInput>();
        }

        public void onUpdate() {
            if (_inputs.Count == 0) return;
            var next = _inputs.First();

            if (next.modifierKey != KeyCode.None && next.state == ModifierState.NotApplied) {
                UnmanagedUtil.keyDown(VirtualKeys.mappedKeys[next.modifierKey]);
                next.state = ModifierState.Applied;
                return;
            }

            if (next.state == ModifierState.Applied) {
                if (Input.GetKeyDown(next.modifierKey))
                    next.state = ModifierState.UseCode;
                return;
            }

            switch (next.type) {
                case ManagedInputType.Keyboard:
                    UnmanagedUtil.keyPressed(next.code);
                    break;
                case ManagedInputType.MouseClick:
                    UnmanagedUtil.mouseClick(next.code);
                    break;
                case ManagedInputType.MouseWheel:
                    UnmanagedUtil.mouseWheelDown();
                    break;
            }
            
            if (next.state == ModifierState.UseCode) {
                if (Input.GetKeyUp(next.key) || Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetMouseButtonUp(next.code))
                    next.state = ModifierState.StopApply;
                return;
            }

            if (next.state == ModifierState.StopApply)
                UnmanagedUtil.keyUp(VirtualKeys.mappedKeys[next.modifierKey]);
            _inputs.Dequeue();
        }

        public void press(KeyCode code, KeyCode modifier = KeyCode.None) {
            var isMouse = code.ToString().ToLower().Contains("mouse");
            var type = isMouse ? ManagedInputType.MouseClick : ManagedInputType.Keyboard;

            _inputs.Enqueue(new ManagedInput(type, code, modifier));
        }

        public void wheel(KeyCode modifier = KeyCode.None) {
            _inputs.Enqueue(new ManagedInput(ManagedInputType.MouseWheel, 0x0, modifier));
        }
    }

    public enum ManagedInputType {
        Keyboard,
        MouseClick,
        MouseWheel
    }

    public enum ModifierState {
        NotApplied,
        Applied,
        UseCode,
        StopApply
    }
    
    public class ManagedInput {
        public readonly ManagedInputType type;
        public readonly int code;
        public readonly KeyCode key;
        public ModifierState state;
        public readonly KeyCode modifierKey;

        public ManagedInput(ManagedInputType type, KeyCode key, KeyCode modifierKey = KeyCode.None) {
            this.type = type;
            this.key = key;
            this.code = VirtualKeys.mappedKeys[key];
            this.modifierKey = modifierKey;
            this.state = ModifierState.NotApplied;
        }
    }
}
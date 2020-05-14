using System;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace AlphaRite.sdk.hacks.gui {
    public class Overlay: AlphaCycle {
        private Rect _windowPosition;
        private bool _windowEnabled;
        private const float WindowWidth = 250;
        private const float WindowHeight = 450;
        private const string PressAnyKey = "Press any key...";

        private bool _waitingForKey1;
        private bool _waitingForKey2;

        public Overlay(AlphariteSdk sdk) : base(sdk, false) {
            var middleX = Screen.width / 2 - WindowWidth / 2;
            var middleY = Screen.height / 2 - WindowHeight / 2;

            _waitingForKey1 = false;
            _waitingForKey2 = false;
            _windowEnabled = false;
            _windowPosition = new Rect(middleX, middleY , WindowWidth, WindowHeight);
        }

        protected override void onUpdate() {
            if (Input.GetKeyDown(KeyCode.Insert))
                _windowEnabled = !_windowEnabled;
        }

        protected override void onRenderingUpdate() {
            checkForKeyboardInputs();
            
            if (_windowEnabled)
                _windowPosition = GUI.Window(0, _windowPosition, buildWindow, "AlphaRite");
        }

        private void buildWindow(int id) {
            GUILayout.Label("Camera Distance");
            var cameraDistance = GUILayout.HorizontalSlider((float) sdk.settings["cameraMaxDistance"], 0f, 100f);
            
            GUILayout.Label("Wallhack");
            if (ToggleButton(sdk.cycleEnabled("wallhack")))
                sdk.toggleCycle("wallhack");
            
            GUILayout.Label("Aimbot - Activation");
            var aimbotHard = sdk.getSetting<bool>("aimbotHard");
            var aimbotHardTarget = sdk.getSetting<bool>("aimbotHardTarget");

            if (ToggleButton(aimbotHard, "ALWAYS", "ON_KEY_PRESSED"))
                sdk.settings["aimbotHard"] = !aimbotHard;
            if (aimbotHard)
                GUILayout.Label("Aimbot - Always mode");
            if (aimbotHard && ToggleButton(aimbotHardTarget, "TARGET", "DIRECTION"))
                sdk.settings["aimbotHardTarget"] = !aimbotHardTarget;

            if (!aimbotHard) {
                GUILayout.Label("Aimbot - Keyboard input Activation");
                if (GUILayout.Button(_waitingForKey1
                        ? PressAnyKey
                        : $"TARGET-LOCK        -    ${sdk.settings["aimbotTargetlockKey"]}") && !_waitingForKey1 &&
                    !_waitingForKey2)
                    _waitingForKey1 = true;

                if (GUILayout.Button(_waitingForKey2
                        ? PressAnyKey
                        : $"DIRECTION-LOCK     -    ${sdk.settings["aimbotDirectionlockKey"]}") && !_waitingForKey1 &&
                    !_waitingForKey2)
                    _waitingForKey2 = true;
                
                GUILayout.Label("Aimbot - Track same Target");
                var keepTarget = sdk.getSetting<bool>("aimbotKeepTarget");
                if (ToggleButton(keepTarget))
                    sdk.settings["aimbotKeepTarget"] = !keepTarget;
            }
            
            GUILayout.Label("Aimbot Max Distance");
            var aimbotDistance = (int) GUILayout.HorizontalSlider(sdk.getSetting<int>("aimbotMaxDistance"), 0f, Screen.height);
            
            if (GUI.changed) {
                sdk.settings["cameraMaxDistance"] = cameraDistance;
                sdk.settings["aimbotMaxDistance"] = aimbotDistance;
            }

            GUI.DragWindow();
        }

        private void checkForKeyboardInputs() {
            var current = Event.current;
            if ((!current.isMouse && !current.isKey) || (!_waitingForKey1 && !_waitingForKey2)) return;
            
            KeyCode code = current.keyCode;
                
            if (current.isMouse)
                code = (KeyCode) Enum.Parse(typeof(KeyCode), "Mouse" + Event.current.button, true);
            
            if (_waitingForKey1) {
                sdk.settings["aimbotTargetlockKey"] = code;
                _waitingForKey1 = false;
            } else if (_waitingForKey2) {
                sdk.settings["aimbotDirectionlockKey"] = code;
                _waitingForKey2 = false;
            }
        }
        
        private bool ToggleButton(bool state, string onTrue = "ON", string onFalse = "OFF") {
            return GUILayout.Button(state ? onTrue : onFalse);
        }
    }
}
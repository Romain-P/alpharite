using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace AlphaRite.sdk.hacks.gui {
    public class Overlay: AlphaCycle {
        private Rect _windowPosition;
        private const float WindowWidth = 250;
        private const float WindowHeight = 450;

        public Overlay(AlphariteSdk sdk) : base(sdk, false) {
            var middleX = Screen.width / 2 - WindowWidth / 2;
            var middleY = Screen.height / 2 - WindowHeight / 2;
            
            _windowPosition = new Rect(middleX, middleY , WindowWidth, WindowHeight);
        }

        protected override void onRenderingUpdate() {
            _windowPosition = GUI.Window(0, _windowPosition, buildWindow, "AlphaRite");
        }

        private void buildWindow(int id) {

            GUILayout.Label("Camera Distance");
            var cameraDistance = GUILayout.HorizontalSlider((float) sdk.settings["cameraMaxDistance"], 0f, 100f);

            if (ToggleButton("Wallhack", sdk.cycleEnabled("wallhack")))
                sdk.toggleCycle("wallhack");

            if (GUI.changed) {
                sdk.settings["cameraMaxDistance"] = cameraDistance;
            }
            
            GUI.DragWindow();
        }

        private bool ToggleButton(string text, bool state) {
            return GUILayout.Button($"{text}    -    {(state ? "ON" : "OFF")}");
        }
    }
}
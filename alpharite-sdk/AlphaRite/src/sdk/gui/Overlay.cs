using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace AlphaRite.sdk.hacks.gui {
    public class Overlay: AlphaCycle {
        private Vector2 _windowPosition;
        private float _builderPositionCache;

        private const float WindowWidth = 250;
        private const float WindowHeight = 450;
        private const float DefaultElementWidth = WindowWidth - 50;
        private const float DefaultElementHeight = 20;

        public Overlay(AlphariteSdk sdk) : base(sdk, false) {
            _windowPosition = Vector2.zero;//new Vector2(Screen.height / 2, Screen.width / 2);
        }

        protected override void onRenderingUpdate() {
            GUI.Window(0, new Rect(_windowPosition.x, _windowPosition.y , WindowWidth, WindowHeight), 
                buildWindow, "AlphaRite");
        }

        private void buildWindow(int id) {
            _builderPositionCache = DefaultElementHeight;
            GUI.DragWindow();

            GUI.Label(nextPosition(), "Camera Distance");
            var cameraDistance = GUI.HorizontalSlider(nextPosition(), (float) sdk.settings["cameraMaxDistance"], 0f, 100f);

            if (Button(nextPosition(), "Wallhack", sdk.cycleEnabled("wallhack"))) {
                Alpharite.println("FUCK YOU TOO");
                sdk.toggleCycle("wallhack");
            }

            if (GUI.changed) {
                sdk.settings["cameraMaxDistance"] = cameraDistance;
            }
        }

        private bool Button(Rect position, string text, bool state) {
            return GUI.Button(position, enableText(text, state));
        }
        
        private string enableText(string text, bool state) {
            return $"{text}    -    {(state ? "ON" : "OFF")}";
        }

        private Rect nextPosition(float width = DefaultElementWidth, float height = DefaultElementHeight) {
            var position = new Rect(10, _builderPositionCache, width, height);

            _builderPositionCache += height;
            return position;
        }
    }
}
using System;
using UnityEngine;

namespace AlphaRite.sdk.hacks.gui {
    public static class UIHelper {

        public static string MakeEnable(string text, bool state) {
            return string.Format("{0}\t-     {1}", text, state ? "ON" : "OFF");
        }

        public static bool Button(float x, float y, float w, float h, string text, bool state) {
            return Button(new Rect(x, y, w, h), MakeEnable(text, state));
        }

        public static bool Button(Rect r, string text) {
            return GUI.Button(r, text);
        }
    }
}
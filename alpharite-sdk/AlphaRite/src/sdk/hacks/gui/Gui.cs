using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlphaRite.sdk.hacks.gui {
    public class Gui: AlphaCycle
    {
        
        private bool _change = false;
        private bool _panelOpen = true;
        private bool _fogOn = false;
        private static string _msg = "";
        
        public Gui(AlphariteSdk sdk) : base(sdk) {
        }
        
        
        protected override void onStart()
        {
            Alpharite.println("GUI: Starting...");
        }

        protected override void onStop()
        {
            Alpharite.println("GUI: Stopping...");
        }
        
        void OnGUI()
        {
            
        }

        private void BuildGui(int id)
        {
            _fogOn = GUI.Toggle(new Rect(10, 20, 180, 20), _fogOn, "Fog toggle");
            if (GUI.changed)
            {
                Alpharite.println("GUI: Fog state - " + _fogOn);
                if (_fogOn)
                    sdk.disableCycle("wallhack");
                else
                    sdk.enableCycle("wallhack");
            }
            GUI.color = Color.red;
            GUI.Label(new Rect(10, 40, 180, 20), "[Didier]");
            GUI.color = Color.yellow;
            GUI.Box(new Rect(10, 60, 180, 20), "4/4 - Distance X cellules");
        
            GUI.color = Color.red;
            GUI.Label(new Rect(10, 80, 180, 20), "[Didier]");
            GUI.color = Color.yellow;
            GUI.Box(new Rect(10, 100, 180, 20), "4/4 - Distance X cellules");
        
            GUI.color = Color.red;
            GUI.Label(new Rect(10, 120, 180, 20), "[Didier]");
            GUI.color = Color.yellow;
            GUI.Box(new Rect(10, 140, 180, 20), "4/4 - Distance X cellules");
        
            GUI.color = Color.white;
            GUI.TextArea(new Rect(10, 170, 180, 150), _msg);
            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, 320, 180, 20), "Alt + C to Close/Open");
            GUI.DragWindow();
        }

        public static void AddMsg(string msg)
        {
            if (_msg.Length != 0)
                msg += "\n";
            _msg = msg + _msg;
        }

        protected override void onRenderingUpdate()
        {
            base.onRenderingUpdate();
            
            if (Input.GetKey(KeyCode.Tab) && Input.GetKey(KeyCode.C) && !_change)
            {
                _panelOpen = !_panelOpen;
                if (!_panelOpen)
                    _msg = "";
                _change = true;
            } else if ((!Input.GetKey(KeyCode.Tab) || !Input.GetKey(KeyCode.C)) && _change)
                _change = false;
        
            if (_panelOpen)
                GUI.Window(0, new Rect(0, 0, 200, 350), BuildGui, "AlphaRite");
        }
    }
}
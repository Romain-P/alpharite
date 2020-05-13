using System;
using System.Collections;
using System.Collections.Generic;
using AlphaRite.sdk.wrappers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlphaRite.sdk.hacks.gui {
    public class Gui: AlphaCycle
    {
        private bool _change = false;
        private bool _panelOpen = true;
        private bool _fogOn = false;
        private static Queue<String> _msg = new Queue<String>();

        private Vector2 _guiPosition = Vector2.zero;
        private Vector2 _offset = Vector2.zero;

        private Vector2 _scrollPosition = Vector2.zero;

        private bool _clicked = false;
        
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
        
        private void BuildGui(int id) {
            if (UIHelper.Button(10, 20, 180, 20, "Wallhack", !_fogOn)) {
                _fogOn = !_fogOn;
                
                if (_fogOn)
                    sdk.disableCycle("wallhack");
                else
                    sdk.enableCycle("wallhack");
            }

            GUI.Label(new Rect(10, 40, 180, 20), "Camera Distance");
            float newValue = GUI.HorizontalSlider(new Rect(10, 60, 180, 20), (float) sdk.settings["cameraMaxDistance"], 0f, 100f);
            if (GUI.changed)
                sdk.settings["cameraMaxDistance"] = newValue;
            var offset = 0;
            try
            {
                if (sdk.refs.players?.enemies != null)
                    foreach (var player in sdk.refs.players.enemies)
                    {
                        if (player.AllyTeam)
                            continue;
                        GUI.color = Color.red;
                        GUI.Label(new Rect(10, 80 + offset, 180, 20), $"[{player.Name}]");
                        GUI.color = Color.yellow;
                        GUI.Box(new Rect(10, 100 + offset, 180, 40), $"Energy {player.Energy}/4\nDistance {player.position().X} {player.position().Y}");
                        offset += 60;
                    }
            }
            catch (Exception e)
            {
                Alpharite.println(e.StackTrace);
            }

            var resultLogs = "";
            foreach (var msg in _msg)
            {
                if (resultLogs.Length > 0)
                    resultLogs += "\n";
                resultLogs += msg;
            }
            GUI.color = Color.white;
            GUI.TextArea(new Rect(10, 100 + offset, 180, 150), resultLogs);

            if (sdk.refs.players?.player != null){
                GUI.color = Color.blue;
                GUI.Label(new Rect(10, 250 + offset, 180, 20), $"Player [{sdk.refs.players.player.Name}]");
                GUI.color = Color.white;
                GUI.Box(new Rect(10, 270 + offset, 180, 20), $"Pos X: {sdk.refs.players.player.position().X}, Y: {sdk.refs.players.player.position().Y}");
                offset += 40;
            }
            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, 250 + offset, 180, 20), "Press insert for enable/disable");
        }

        public static void AddMsg(string msg)
        {
            if (_msg.Count > 30)
                _msg.Dequeue();
            _msg.Enqueue(msg);
        }

        protected override void onRenderingUpdate()
        {
            var curPointer = new Vector2(Input.mousePosition.x, (Camera.main.pixelHeight - Input.mousePosition.y));
            if (Input.GetKeyDown(KeyCode.Mouse0) && curPointer.x >= _guiPosition.x + _offset.x &&
                curPointer.x <= 200 + _guiPosition.x + _offset.x && curPointer.y >= _guiPosition.y + _offset.y &&
                curPointer.y <= 350 + _guiPosition.y + _offset.y && !_clicked)
            {
                _clicked = true;
                _offset = new Vector2(_guiPosition.x + _offset.x - curPointer.x, _guiPosition.y + _offset.y - curPointer.y);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
                _clicked = false;
            
            if (_clicked)
                _guiPosition = new Vector2(curPointer.x, curPointer.y);
            if (Input.GetKey(KeyCode.Insert) && !_change)
            {
                _panelOpen = !_panelOpen;
                _change = true;
                sdk.disableCycle("aimbot");
            } else if ((!Input.GetKey(KeyCode.Insert)) && _change)
                _change = false;
    
            if (_panelOpen)
                GUI.Window(0, new Rect(_guiPosition.x + _offset.x, _guiPosition.y + _offset.y, 200, 400), BuildGui, "AlphaRite");
        }
    }
}
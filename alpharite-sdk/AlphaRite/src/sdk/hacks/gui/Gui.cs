using System;
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
        private static string _msg = "";

        private Vector2 _guiPosition = new Vector2(0, 0);
        private Vector2 _offset = new Vector2(0, 0);

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

            var offset = 0;
            try
            {
                if (sdk.refs.players?.player != null)
                    Alpharite.println($"X: {sdk.refs.players.player.position().X}, Y: {sdk.refs.players.player.position().Y}");
                if (sdk.refs.players?.enemies != null)
                    foreach (var player in sdk.refs.players.enemies)
                    {
                        if (!player.AllyTeam)
                            continue;
                        GUI.color = Color.red;
                        GUI.Label(new Rect(10, 40 + offset, 180, 20), $"[WIP]");
                        GUI.color = Color.yellow;
                        GUI.Box(new Rect(10, 60 + offset, 180, 20), $"4/4 - Distance {player.position().X} {player.position().Y} cellules");
                        offset += 40;
                    }
            }
            catch (Exception e)
            {
                Alpharite.println(e.StackTrace);
            }

            GUI.color = Color.white;
            GUI.TextArea(new Rect(10, 50, 180, 150), _msg);
            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, 200, 180, 20), "Alt + C to Close/Open");
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
            if (Input.GetKey(KeyCode.Tab) && Input.GetKey(KeyCode.C) && !_change)
            {
                _panelOpen = !_panelOpen;
                if (!_panelOpen)
                    _msg = "";
                _change = true;
            } else if ((!Input.GetKey(KeyCode.Tab) || !Input.GetKey(KeyCode.C)) && _change)
                _change = false;
    
            if (_panelOpen)
                GUI.Window(0, new Rect(_guiPosition.x + _offset.x, _guiPosition.y + _offset.y, 200, 350), BuildGui, "AlphaRite");
        }
    }
}
﻿using System;
 using System.Collections.Generic;
using AlphaRite.sdk.hacks;
 using AlphaRite.sdk.scripts.gui;
 using HarmonyLib;
 using MergedUnity.Glues;
 using MergedUnity.Glues.GUI;
 using UnityShared;

 namespace AlphaRite.sdk {
    public class AlphariteSdk {
        private Dictionary<string, AlphaCycle> _cycles;
        public ReferenceHolder refs { get; }
        public Harmony patcher { get; }
        
        public AlphariteSdk() {
            patcher = new Harmony("AlphaRite.sdk");
            
            _cycles = new Dictionary<string, AlphaCycle>();
            refs = new ReferenceHolder();
            
            subscribeHacks();
            subscribeScripts();
        }

        public void enableCycle(string cycleAlias) {
            var cycle = _cycles[cycleAlias] ?? throw new Exception("Cycle {0} does not exist".format(cycleAlias));
            cycle.enable();
            Alpharite.println("Enabled {0} successfully", cycleAlias);
        }
        
        public void disableCycle(string cycleAlias) {
            var cycle = _cycles[cycleAlias] ?? throw new Exception("Cycle {0} does not exist".format(cycleAlias));
            cycle.disable();
            Alpharite.println("Disabled {0} successfully", cycleAlias);
        }

        void subscribeHacks() {
            _cycles["wallhack"] = new WallHack(this);
            _cycles["aimbot"] = new Aimbot(this);
        }

        void subscribeScripts() {
            _cycles["gui"] = new GuiScript(this);
            // TODO
        }

        public void onStart() {
            enableDebugTools();
            
            foreach (var cycle in _cycles.Keys)
               enableCycle(cycle);
        }

        public void onStop() {
            foreach (var cycle in _cycles.Keys)
                disableCycle(cycle);
        }

        public void onRenderingUpdate() {
            foreach (var cycle in _cycles.Values)
                cycle.renderingUpdate();
        }

        public void onUpdate() {
            foreach (var cycle in _cycles.Values)
                cycle.update();
        }

        private void enableDebugTools() {
            CommandLineSettings.Settings.ShowDebugData = true;
            CommandLineSettings.Settings.UseConsole = true;
            
            GUIGlobals.Glue.RemoveRef(typeof(StunConsoleInstaniatorGlue));
            GUIGlobals.Glue.RemoveRef(typeof(DebugDataViewInstaniatorGlue));
            
            GUIGlobals.Glue.AddRef(int.MaxValue, typeof(StunConsoleInstaniatorGlue), typeof(DebugDataViewInstaniatorGlue));
            GUIGlobals.Glue.Get<StunConsoleInstaniatorGlue>().DoInitialize();
            GUIGlobals.Glue.Get<DebugDataViewInstaniatorGlue>().DoInitialize();
            
            GUIGlobals.Glue.Update();
        }
    }
}
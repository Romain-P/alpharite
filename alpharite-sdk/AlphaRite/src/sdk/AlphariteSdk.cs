 ﻿using System;
  using System.Collections.Generic;
  using AlphaRite.sdk.hacks;
  using AlphaRite.sdk.hacks.gui;
  using HarmonyLib;
  using MergedUnity.Glues;
  using MergedUnity.Glues.GUI;
  using UnityShared;

  namespace AlphaRite.sdk {

     public class AlphariteSdk {
        private Dictionary<string, AlphaCycle> _cycles;
        public InputHandler inputHandler { get; }
        public Dictionary<string, object> settings { get; }
        public ReferenceHolder refs { get; }
        public Harmony patcher { get; }
        
        public AlphariteSdk() {
            patcher = new Harmony("AlphaRite.sdk");
            
            _cycles = new Dictionary<string, AlphaCycle>();
            inputHandler = new InputHandler();
            settings = new Dictionary<string, object>();
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

        public void toggleCycle(string cycleAlias) {
            var cycle = _cycles[cycleAlias] ?? throw new Exception("Cycle {0} does not exist".format(cycleAlias));
            
            if (cycle.enabled)
                disableCycle(cycleAlias);
            else
                enableCycle(cycleAlias);
        }

        public bool cycleEnabled(string cycleAlias) {
            var cycle = _cycles[cycleAlias] ?? throw new Exception("Cycle {0} does not exist".format(cycleAlias));
            return cycle.enabled;
        }

        void subscribeHacks() {
            _cycles["wallhack"] = new WallHack(this);
            _cycles["aimbot"] = new Aimbot(this);
            _cycles["cameraUnlock"] = new CameraUnlock(this);
            _cycles["gui"] = new Overlay(this);
        }

        void subscribeScripts() {
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
            inputHandler.onUpdate();
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
        
        public T getSetting<T>(string name) {
            return (T) settings[name];
        } 
    }
}
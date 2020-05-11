﻿using System.Collections.Generic;
using AlphaRite.sdk.hacks;
 using HarmonyLib;
 using MergedUnity.Glues;
 using MergedUnity.Glues.GUI;
 using UnityShared;

 namespace AlphaRite.sdk {
    public class AlphariteSdk {
        private List<AlphaCycle> _cycles;
        public ReferenceHolder refs { get; }
        public Harmony patcher { get; }
        
        public AlphariteSdk() {
            patcher = new Harmony("AlphaRite.sdk");
            
            _cycles = new List<AlphaCycle>();
            refs = new ReferenceHolder();
            
            subscribeHacks();
            subscribeScripts();
        }

        void subscribeHacks() {
            _cycles.Add(new WallHack(this));
            _cycles.Add(new Aimbot(this));
        }

        void subscribeScripts() {
            // TODO
        }

        public void onStart() {
            enableDebugTools();
            
            foreach (var cycle in _cycles)
                cycle.enable();
        }

        public void onStop() {
            foreach (var cycle in _cycles)
                cycle.disable();
        }

        public void onRenderingUpdate() {
            foreach (var cycle in _cycles)
                cycle.renderingUpdate();
        }

        public void onUpdate() {
            foreach (var cycle in _cycles)
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
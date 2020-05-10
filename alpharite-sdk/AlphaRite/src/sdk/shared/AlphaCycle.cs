﻿using System;
 using System.Collections.Generic;
 using System.Reflection;
 using HarmonyLib;

 namespace AlphaRite.sdk {
    public abstract class AlphaCycle {
        private bool _enabled;
        private Dictionary<String, Hook> methods;
        protected AlphariteSdk sdk;
        protected Harmony patcher;
        
        public AlphaCycle(AlphariteSdk sdk) {
            this.methods = new Dictionary<string, Hook>();
            this._enabled = false;
            this.sdk = sdk;
            this.patcher = sdk.patcher;
        }

        protected void enableHook(string alias) {
            var hook = methods[alias];
            patcher.Patch(hook.originalMethod, hook.hookMethod);
        }

        protected void disableHook(string alias) {
            var hook = methods[alias];
            patcher.Unpatch(hook.originalMethod, hook.hookMethod.method);
        }

        protected void hookMethod<TO, TH>(string alias, string originalMethod, string hookMethod) {
            var detour = new Hook(typeof(TO).GetMethod(originalMethod), 
                new HarmonyMethod(typeof(TH).GetMethod(hookMethod)));
            
            methods[alias] = detour;
        }
        
        protected void hookGetProperty<TO, TH>(string alias, string original, string hook) {
            var detour = new Hook(typeof(TO).GetProperty(original)?.GetGetMethod(), 
                new HarmonyMethod(typeof(TH).GetMethod(hook)));
            
            methods[alias] = detour;
        }
        
        protected void hookSetProperty<TO, TH>(string alias, string original, string hook) {
            var detour = new Hook(typeof(TO).GetProperty(original)?.GetSetMethod(), 
                new HarmonyMethod(typeof(TH).GetMethod(hook)));
            
            methods[alias] = detour;
        }

        protected MethodInfo original(string alias) {
            return methods[alias].originalMethod;
        }

        protected HarmonyMethod hook(string alias) {
            return methods[alias].hookMethod;
        }

        protected abstract void onStart();
        protected abstract void onStop();
        protected virtual void onUpdate() {}
        protected virtual void onRenderingUpdate() {}
        
        public void enable() {
            if (_enabled) return;
            onStart();
            _enabled = true;
        }

        public void disable() {
            if (!_enabled) return;
            onStop();
            _enabled = false;
        }

        public void update() {
            if (!_enabled) return;
            onUpdate();
        }

        public void renderingUpdate() {
            if (!_enabled) return;
            onRenderingUpdate();
        }
    }
}
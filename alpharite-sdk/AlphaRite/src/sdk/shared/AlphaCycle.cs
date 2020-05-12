﻿using System;
 using System.Collections.Generic;
 using System.Reflection;
 using HarmonyLib;

 namespace AlphaRite.sdk {
    public abstract class AlphaCycle {
        private bool _enabled;
        private Dictionary<String, Hook> _methods;
        private bool _requiresInMatch;
        protected AlphariteSdk sdk;
        protected Harmony patcher;
        
        public AlphaCycle(AlphariteSdk sdk, bool requiresInMatch = true) {
            this._methods = new Dictionary<string, Hook>();
            this._enabled = false;
            this.sdk = sdk;
            this.patcher = sdk.patcher;
            this._requiresInMatch = requiresInMatch;
        }

        protected void enableHook(string alias) {
            var hook = _methods[alias];
            switch (hook.mode) {
                case Hook.HookMode.PREFIX:
                    patcher.Patch(hook.originalMethod, prefix: hook.hookMethod);
                    break;
                case Hook.HookMode.POSTFIX:
                    patcher.Patch(hook.originalMethod, postfix: hook.hookMethod);
                    break;
            }
        }

        protected void disableHook(string alias) {
            var hook = _methods[alias];
            patcher.Unpatch(hook.originalMethod, hook.hookMethod.method);
        }

        protected void hookMethod<TO, TH>(string alias, string originalMethod, string hookMethod, Hook.HookMode mode = Hook.HookMode.PREFIX) {
            var detour = new Hook(typeof(TO).getMethod(originalMethod), 
                new HarmonyMethod(typeof(TH).getMethod(hookMethod)), mode);
            
            _methods[alias] = detour;
        }
        
        protected void hookGetProperty<TO, TH>(string alias, string original, string hook, Hook.HookMode mode = Hook.HookMode.PREFIX) {
            var detour = new Hook(typeof(TO).getProperty(original).GetGetMethod(), 
                new HarmonyMethod(typeof(TH).getMethod(hook)), mode);
            
            _methods[alias] = detour;
        }
        
        protected void hookSetProperty<TO, TH>(string alias, string original, string hook, Hook.HookMode mode = Hook.HookMode.PREFIX) {
            var detour = new Hook(typeof(TO).getProperty(original).GetSetMethod(), 
                new HarmonyMethod(typeof(TH).getMethod(hook)), mode);
            
            _methods[alias] = detour;
        }

        protected MethodInfo original(string alias) {
            return _methods[alias].originalMethod;
        }

        protected HarmonyMethod hook(string alias) {
            return _methods[alias].hookMethod;
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
            if (!_enabled || !respectsConstraints) return;
            onUpdate();
        }

        public void renderingUpdate() {
            if (!_enabled || !respectsConstraints) return;
            onRenderingUpdate();
        }

        private bool respectsConstraints => !_requiresInMatch || _requiresInMatch && sdk.refs.inMatch;
    }
}
using System.Reflection;
using HarmonyLib;

namespace AlphaRite.sdk {
    public struct Hook {
        public enum HookMode {
            POSTFIX,
            PREFIX
        }
        
        public MethodInfo originalMethod { get; }
        public HarmonyMethod hookMethod { get; }
        public HookMode mode { get; }

        public Hook(MethodInfo originalMethod, HarmonyMethod hookMethod, HookMode mode = HookMode.PREFIX) {
            this.originalMethod = originalMethod;
            this.hookMethod = hookMethod;
            this.mode = mode;
        }
    }
}
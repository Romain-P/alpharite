using System.Reflection;
using HarmonyLib;

namespace AlphaRite.sdk {
    public struct Hook {
        public MethodInfo originalMethod { get; }
        public HarmonyMethod hookMethod { get; }

        public Hook(MethodInfo originalMethod, HarmonyMethod hookMethod) {
            this.originalMethod = originalMethod;
            this.hookMethod = hookMethod;
        }
    }
}
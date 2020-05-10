 using Gameplay;
 using UnityEngine;

 namespace AlphaRite.sdk.hacks {
    public class WallHack: AlphaCycle {
        public WallHack(AlphariteSdk sdk) : base(sdk) {
            registerPropertyGet<GameClient, WallHack>("wallhack", "FogOfWarEnabled", "WallhackHook");
        }

        protected override void onStart() {
            enableHook("wallhack");
            Alpharite.println("Applied Hook");
        }

        protected override void onStop() {
            disableHook("wallhack");
            Alpharite.println("Removed Hook");
        }

        public static bool WallhackHook(ref bool __result) {
            __result = false;
            return false;
        }
    }
}
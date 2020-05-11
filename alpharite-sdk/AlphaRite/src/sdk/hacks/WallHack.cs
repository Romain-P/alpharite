 using BloodGUI;
 using BloodGUI_Binding.HUD;
 using Gameplay;
 using UnityEngine;

 namespace AlphaRite.sdk.hacks {
    public class WallHack: AlphaCycle {
        public WallHack(AlphariteSdk sdk) : base(sdk) {
            hookGetProperty<GameClient, WallHack>("wallhack", "FogOfWarEnabled", "WallhackHook");
        }

        protected override void onStart() {
            enableHook("wallhack");
        }

        protected override void onStop() {
            disableHook("wallhack");
        }

        public static bool WallhackHook(ref bool __result) {
            __result = false;
            return false;
        }
    }
}
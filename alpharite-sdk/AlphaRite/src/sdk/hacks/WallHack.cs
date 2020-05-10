﻿using System.Threading;
using Gameplay;
using Gameplay.GameObjects;
 using HarmonyLib;
 using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using StunShared.GlueSystem;
 using UnityEngine;

 namespace AlphaRite.sdk.hacks {
    public class WallHack: AlphaCycle {
        public WallHack(AlphariteSdk sdk) : base(sdk) {
            registerPropertyGet<GameClient, WallHack>("wallhack", "FogOfWarEnabled", "WallhackHook");
        }

        protected override void onStart() {
            enableHook("wallhack");
            Debug.LogError("Applied Hook");
        }

        protected override void onStop() {
            disableHook("wallhack");
            Debug.LogError("Removed Hook");
        }

        public static bool WallhackHook(ref bool __result) {
            __result = false;
            return false;
        }
    }
}
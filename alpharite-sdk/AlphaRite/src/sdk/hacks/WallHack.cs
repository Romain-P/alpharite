﻿using System.Threading;
using Gameplay;
using Gameplay.GameObjects;
using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using StunShared.GlueSystem;
 using UnityEngine;

 namespace AlphaRite.sdk.hacks {
    public class WallHack: AlphaCycle {
        public WallHack(AlphariteSdk sdk) : base(sdk) {}

        protected override void onStart() {
            Debug.LogError("created");
        }

        protected override void onStop() {
            Debug.LogError("stopped");
        }

        protected override void onUpdate() {
            Debug.LogError("test");
            //sdk.references.gameClient.Instance.SendChatMessage("fuck you", true);
        }
    }
}
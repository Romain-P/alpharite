using System;
using AlphaRite.sdk.wrappers;
using BloodGUI_Binding.Base;
using Gameplay;
using GeneralVR;
using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using RemoteClient;
using StunShared.GlueSystem;
using UnityEngine;

namespace AlphaRite.sdk.hacks {
    public class Aimbot: AlphaCycle {
        public Aimbot(AlphariteSdk sdk): base(sdk) {}

        protected override void onStart() {
        }

        protected override void onUpdate() {
            //if (!sdk.refs.client.IsConnected) return;
            
            //Alpharite.println("position: {0}", sdk.refs.players.playerPosition);
        }

        protected override void onStop() {
            
        }
    }
}
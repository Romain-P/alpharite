﻿using System.Threading;
using Gameplay;
using Gameplay.GameObjects;
using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using StunShared.GlueSystem;

namespace AlphaRite.sdk.hacks {
    public class WallHack: AlphaCycle {
        public WallHack(AlphariteSdk sdk) : base(sdk) {}

        protected override void onStart() {
            DataTableSystem dataTableSystem = sdk.references.datatableSystem.Instance;
            var data = new DataTable(dataTableSystem, dataTableSystem.Create(GameObjectTypeId.Empty));
            
            data.Set("Enabled", false);
            sdk.references.gameClient.Instance.Debug("fogofwar", data, false);
        }

        protected override void onStop() {
            DataTableSystem dataTableSystem = sdk.references.datatableSystem.Instance;
            var data = new DataTable(dataTableSystem, dataTableSystem.Create(GameObjectTypeId.Empty));
            
            data.Set("Enabled", true);
            sdk.references.gameClient.Instance.Debug("fogofwar", data, false);
        }

        protected override void onUpdate() {
            sdk.references.gameClient.Instance.SendChatMessage("fuck you", true);
        }
    }
}
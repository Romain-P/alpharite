using System;
using System.Collections.Generic;
using AlphaRite.sdk.wrappers;
using Avro;
using BloodGUI;
using BloodGUI_Binding.Base;
using BloodGUI_Binding.HUD;
using Gameplay;
using Gameplay.DataIO;
using Gameplay.GameObjects;
using Gameplay.View;
using MathCore;
using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using ns18;
using RemoteClient;
using StunShared.GlueSystem;
using UnityShared;

namespace AlphaRite.sdk {
    public class ReferenceHolder {
        public static ReferenceHolder instance;

        public ReferenceHolder() {
            instance = this;
        }

        public K glueInstance<T, K>(LoadingState minimumLoadingState = LoadingState.Ready) where T: InstancingGlue<K> {
            return GUIGlobals.Glue.Get<T>(minimumLoadingState).Instance;
        }

        public IGameClient clientInterface => glueInstance<GameClientGlue, IGameClient>();

        public GameClient client => clientInterface.GetGame();

        public ViewState viewState => clientInterface.GetViewState();
        
        public UI_HUDBase hud => glueInstance<HUDBaseGlue, UI_HUDBase>();
        
        public IGameplayData data => glueInstance<GameplayDataGlue, IGameplayData>();

        public PlayerWrapper.References players => new PlayerWrapper.References();
    }
}
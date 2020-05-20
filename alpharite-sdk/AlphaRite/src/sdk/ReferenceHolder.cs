
using AlphaRite.sdk.wrappers;
using BloodGUI_Binding.Base;
using CollisionLibrary;
using Gameplay;
using Gameplay.View;
using JetBrains.Annotations;
using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using RemoteClient;
using StunShared.GlueSystem;
using UnityEngine;

namespace AlphaRite.sdk {
    public class ReferenceHolder {
        public static ReferenceHolder instance;

        public ReferenceHolder() {
            instance = this;
        }

        public K glueInstance<T, K>(LoadingState minimumLoadingState = LoadingState.Ready) where T: InstancingGlue<K> {
            var glue = GUIGlobals.Glue.Get<T>(minimumLoadingState);
            return glue == null ? default : glue.Instance;
        }

        [CanBeNull] public IGameClient clientInterface => glueInstance<GameClientGlue, IGameClient>();

        [CanBeNull] public GameClient client => clientInterface?.GetGame();

        [CanBeNull] public ViewState viewState => clientInterface?.GetViewState();
        
        public UI_HUDBase hud => glueInstance<HUDBaseGlue, UI_HUDBase>();
        
        public IGameplayData data => glueInstance<GameplayDataGlue, IGameplayData>();

        public PlayerWrapper.References players => new PlayerWrapper.References();

        [CanBeNull] public Camera camera => GUIGlobals.Glue.Get<GameplayCameraGlue>()?.Instance.Camera;

        public MapObjectWrapper mapObjects => new MapObjectWrapper();

        public bool inMatch => viewState != null && !viewState.IsLoading;

        public Pathfinder pathfinder => client.getPropertyValue<Pathfinder>("Pathfinding");
    }
}
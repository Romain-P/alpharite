using Gameplay;
using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using ns10;
using RemoteClient;
using StunShared.GlueSystem;

namespace AlphaRite.sdk {
    public class ReferenceHolder {
        
        public ReferenceHolder() {}

        public K glueInstance<T, K>(LoadingState minimumLoadingState = LoadingState.Ready) where T: InstancingGlue<K> {
            return GUIGlobals.Glue.Get<T>(minimumLoadingState).Instance;
        }

        public GameClient gameClient {
            get {
                var gameClientProxy = glueInstance<GameClientGlue, IGameClient>();

                return Reflection.proxy<Class3157, GameClient>(gameClientProxy)
                    .deep<Class3159>("igameClient_0")
                    .deepAndResolve("gameClient_0");
            }
        }
    }
}
using MergedUnity.Glues;
using MergedUnity.Glues.GUI;
using StunShared.GlueSystem;

namespace AlphaRite.sdk {
    public class ReferenceHolder {
        
        public ReferenceHolder() {}

        public T get<T>(LoadingState minimumLoadingState = LoadingState.Ready) where T: InstancingGlue<T> {
            return GUIGlobals.Glue.Get<T>(minimumLoadingState).Instance;
        }
    }
}
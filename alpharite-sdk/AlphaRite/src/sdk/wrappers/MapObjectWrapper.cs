using Gameplay.View;

namespace AlphaRite.sdk.wrappers {
    public class MapObjectWrapper: Wrapper {
        public ActiveObject? retrieve(string name) {
            for (var i = 0; i < refs.viewState?.ActiveObjects.Count; i++) {
                var it = refs.viewState.ActiveObjects.Values[i];

                if (refs.data.GetTypeName(it.TypeId) == name)
                    return it;
            }

            return null;
        }
    }
}
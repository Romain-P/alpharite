using MergedUnity.Glues;
using MergedUnity.Glues.GUI;

namespace AlphaRite.sdk {
    public class ReferenceHolder {
        public GameClientGlue gameClient { get; }
        public DataTableSystemGlue datatableSystem { get; }
        
        public ReferenceHolder() {
            gameClient = GUIGlobals.Glue.Get<GameClientGlue>();
            datatableSystem = GUIGlobals.Glue.Get<DataTableSystemGlue>();
        }
    }
}
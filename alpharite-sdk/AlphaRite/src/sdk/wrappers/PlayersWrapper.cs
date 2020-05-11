using System.Collections.Generic;
using BloodGUI;
using BloodGUI_Binding.HUD;
using Gameplay;
using Gameplay.GameObjects;
using MathCore;

namespace AlphaRite.sdk.wrappers {
    public static class PlayersWrapper {
        private static readonly ReferenceHolder refs = ReferenceHolder.instance;
        
        //////////////////
        ////References////
        //////////////////
        public class References: Wrapper {
            public UI_PlayersInfoBinding unwrapped =>
                refs.hud.getFieldValue<UI_PlayersInfoBinding>("_PlayersInfoBinding");

            public List<Data_PlayerInfo> enemies => unwrapped.getFieldValue<List<Data_PlayerInfo>>("_EnemyTeamData");
            public List<Data_PlayerInfo> allies => unwrapped.getFieldValue<List<Data_PlayerInfo>>("_LocalTeamData");
            
            public Data_PlayerInfo player {
                get {
                    //player only present in Arena mode
                    Data_PlayerInfo infos = allies.Find(x => x.LocalPlayer);

                    //find player manually if not in cache (e.g happens in Playground mode)
                    if (infos.ID.Index == 0) {
                        var states = refs.viewState.HudStates.method_1("PartyBarCharacter");

                        foreach (var state in states) {
                            var table = state.Data;
                            var id = (GameObjectId) table.Get("ID");

                            infos.ID = new UIGameObjectId(id.Index, id.Generation);
                            infos.LocalPlayer = true;//table.Get("IsLocalPlayer");
                            break;
                        }
                    }

                    return infos;
                }
            }
        }

        //////////////////
        ////Extensions////
        //////////////////
        public static Vector2 position(this Data_PlayerInfo self) {
            return new GameObjectId(self.ID.Index, self.ID.Generation).position();
        }
        
        public static Vector2 position(this GameObjectId self) {
            return refs.client.GetState(self, "Position", false);
        }
    }
}
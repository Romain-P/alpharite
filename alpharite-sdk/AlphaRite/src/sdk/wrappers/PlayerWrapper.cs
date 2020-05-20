using System.Collections.Generic;
using BloodGUI;
using BloodGUI_Binding.HUD;
using Gameplay;
using Gameplay.GameObjects;
using Gameplay.View;
using UnityEngine;
using Vector2 = MathCore.Vector2;

namespace AlphaRite.sdk.wrappers {
    public static class PlayerWrapper {
        private static readonly ReferenceHolder refs = ReferenceHolder.instance;
        
        //////////////////
        ////References////
        //////////////////
        public class References: Wrapper {
            public UI_PlayersInfoBinding unwrapped => refs.hud.getFieldValue<UI_PlayersInfoBinding>("_PlayersInfoBinding");
            public List<Data_PlayerInfo> enemies => unwrapped.getFieldValue<List<Data_PlayerInfo>>("_EnemyTeamData");
            public List<Data_PlayerInfo> allies => unwrapped.getFieldValue<List<Data_PlayerInfo>>("_LocalTeamData");
            
            public Data_PlayerInfo player {
                get {
                    //player only present in Arena mode
                    Data_PlayerInfo infos = allies.Find(x => x.LocalPlayer);
                    
                    //find player manually if not in cache (e.g happens in Playground mode)
                    if (infos.ID.Index != 0) return infos;
                    
                    var states = refs?.viewState?.HudStates?.method_1("PartyBarCharacter") ?? default;

                    foreach (var state in states) {
                        var table = state.Data;
                        var id = (GameObjectId) table.Get("ID");

                        infos.ID = new UIGameObjectId(id.Index, id.Generation);
                        infos.LocalPlayer = true;//table.Get("IsLocalPlayer");
                        break;
                    }

                    return infos;
                }
            }
            
            public Data_PlayerInfo nearestEnemyFromCursor {
                get {
                    Data_PlayerInfo nearest = default;
                    var lowestDist = float.MaxValue;

                    var mousePosition = Input.mousePosition.toDualDimension();
                    foreach (var p in refs.players.enemies) {
                        if (!p.isValid()) continue;
                        
                        var pos = p.screenPosition().toDualDimension() - mousePosition;
                        var dist = pos.magnitude();

                        if (dist >= lowestDist) continue;

                        lowestDist = dist;
                        nearest = p;
                    }

                    return nearest;
                }
            }
        }

        //////////////////
        ////Extensions////
        //////////////////
        public static Vector2 position(this Data_PlayerInfo self, float time = 0f) {
            return new GameObjectId(self.ID.Index, self.ID.Generation).position(time);
        }
        
        public static Vector2 position(this GameObjectId self, float time = 0f) {
            //real-time
            Vector2 pos = refs?.client?.GetState(self, "Position", false) ?? default;
            if (time <= 0) return pos;
            
            //prediction
            Vector2 velocity = refs?.client?.GetState(self, "Velocity", false) ?? default;
            return pos + velocity.Normalized * time;
        }

        public static Vector3 screenPosition(this GameObjectId self, float time = 0f) {
            var position = self.position(time);
            var adjusted = refs.camera.WorldToScreenPoint(new Vector3(position.X, 0, position.Y));

            adjusted.y = Screen.height - adjusted.y;
            return adjusted;
        }

        public static Vector3 screenPosition(this Data_PlayerInfo self, float time = 0f) {
            var position = self.position(time);
            var adjusted = refs.camera.WorldToScreenPoint(new Vector3(position.X, 0, position.Y));

            adjusted.y = Screen.height - adjusted.y;
            return adjusted;
        }

        public static Vector2 toDualDimension(this Vector3 self) {
            return new Vector2(self.x, self.y);
        }
        
        public static UnityEngine.Vector2 toDualDimensionUnity(this Vector3 self) {
            return new UnityEngine.Vector2(self.x, self.y);
        }

        public static bool isValid(this Data_PlayerInfo self, Vector2? validFrom = null) {
            var from = validFrom ?? refs.players.player.position();
            
            return !self.IsDead && !self.IsDisconnected && refs.pathfinder.CanSee(self.position(), from, 0.5f);
        }
    }
}
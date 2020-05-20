using System;
using WindowsInput;
using WindowsInput.Native;
using AlphaRite.sdk.wrappers;
using BloodGUI;
using Gameplay.GameObjects;
using Gameplay.View;
using UnityEngine;

namespace AlphaRite.sdk.hacks {
    public class Aimbot: AlphaCycle {
        private float _aimMagnitudeCache;
        private Data_PlayerInfo _aimTargetCache;
        
        public Aimbot(AlphariteSdk sdk): base(sdk) {}

        protected override void onStart() {
            sdk.settings["aimbotHard"] = false;
            sdk.settings["aimbotMaxDistance"] = Screen.width / 2;
            sdk.settings["aimbotHardTarget"] = false;
            sdk.settings["aimbotDirectionlockKey"] = KeyCode.LeftAlt;
            sdk.settings["aimbotTargetlockKey"] = KeyCode.W;
            sdk.settings["aimbotKeepTarget"] = true;
        }

        protected override void onUpdate() {
            var aimTargetKey = sdk.getSetting<KeyCode>("aimbotTargetlockKey");
            var aimDirectionKey = sdk.getSetting<KeyCode>("aimbotDirectionlockKey");

            var alwaysMode = sdk.getSetting<bool>("aimbotHard");
            var alwaysModeTarget = sdk.getSetting<bool>("aimbotHardTarget");
            var targetKeyPressed = Input.GetKey(aimTargetKey);
            var directionKeyPressed = Input.GetKey(aimDirectionKey);
            var refreshPosition = Input.GetKeyDown(aimTargetKey) || Input.GetKeyDown(aimDirectionKey);

            if (directionKeyPressed || (alwaysMode && !alwaysModeTarget))
                aimbotActivated(false, refreshPosition);
            else if (targetKeyPressed || alwaysMode)
                aimbotActivated(true, refreshPosition);
        }

        private void aimbotActivated(bool hardLock, bool refreshPosition) {
            var str = "";
            for (var i = 0; i < sdk.refs.viewState.ActiveObjects.Count; i++) {
                var type = sdk.refs.viewState.ActiveObjects.Values[i].TypeId;

                str += sdk.refs.data.GetTypeName(type) + " - ";
            }
            Alpharite.println(str);
            if (!sdk.getSetting<bool>("aimbotKeepTarget") || refreshPosition || !_aimTargetCache.isValid()) {
                var target = sdk.refs.players.nearestEnemyFromCursor;

                if (target.ID.Index > 0)
                    _aimTargetCache = target;
            }
            var screenPosition = _aimTargetCache.screenPosition(1f);
            var distance = (sdk.refs.players.player.screenPosition() - screenPosition).magnitude;

            if (!onScreenArea(screenPosition) || distance > sdk.getSetting<int>("aimbotMaxDistance"))
                return;
            if (hardLock)
                lockAim(screenPosition.x, screenPosition.y);
            else
                lockAimOnDirection(screenPosition);
        }

        private void lockAimOnDirection(Vector3 targetScreenPosition) {
            var playerPosition = sdk.refs.players.player.screenPosition().toDualDimensionUnity();
            var mousePosition = Input.mousePosition.toDualDimensionUnity();
            var targetPosition = targetScreenPosition.toDualDimensionUnity();

            var playerVector = mousePosition - playerPosition;
            var targetVector = targetPosition - playerPosition;

            if (Input.GetKeyDown(KeyCode.LeftAlt))
                _aimMagnitudeCache = Math.Max(playerVector.magnitude, 300);

            var point = playerPosition + (_aimMagnitudeCache * targetVector.normalized);
            lockAim(point.x, point.y);
        }

        private void lockAim(float x, float y) {
            const int flagMove = (int) UnmanagedUtil.MouseEvent.MOUSEEVENTF_MOVE;
            const int flagAbsolute = (int) UnmanagedUtil.MouseEvent.MOUSEEVENTF_ABSOLUTE;
            var screenX = (int) (x / Screen.width * 65535f);
            var screenY = (int) (y / Screen.height * 65535f);
            
            UnmanagedUtil.mouse_event(flagMove | flagAbsolute, screenX, screenY, 0, 0);
        }

        private bool onScreenArea(Vector3 screenPosition) {
            return screenPosition.x > 0 && screenPosition.x < Screen.width &&
                   screenPosition.y > 0 && screenPosition.y < Screen.height;
        }

        protected override void onStop() {
            
        }
    }
}
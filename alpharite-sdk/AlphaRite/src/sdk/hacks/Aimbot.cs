using System;
using AlphaRite.sdk.wrappers;
using UnityEngine;

namespace AlphaRite.sdk.hacks {
    public class Aimbot: AlphaCycle {
        public Aimbot(AlphariteSdk sdk): base(sdk) {}

        protected override void onStart() {
        }

        protected override void onUpdate() {
            var target = sdk.refs.players.nearestEnemyFromCursor;

            if (target.ID.Index > 0 && Input.GetKey(KeyCode.LeftAlt)) {
                var pos = target.screenPosition();
                lockAim(pos.x, pos.y);
            }
        }

        //TODO: smooth
        private void lockAimOnDirection(Vector3 targetScreenPosition) {
            var playerPosition = sdk.refs.players.player.screenPosition().toDualDimensionUnity();
            var mousePosition = Input.mousePosition.toDualDimensionUnity();
            var targetPosition = targetScreenPosition.toDualDimensionUnity();

            var playerVector = mousePosition - playerPosition;
            var targetVector = targetPosition - playerPosition;

            var point = playerPosition + (playerVector.magnitude * targetVector.normalized);
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
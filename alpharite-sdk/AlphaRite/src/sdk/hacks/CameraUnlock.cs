 using Gameplay.View;

 namespace AlphaRite.sdk.hacks {
    public class CameraUnlock: AlphaCycle {
        public CameraUnlock(AlphariteSdk sdk) : base(sdk) {}

        protected override void onStart() {
            sdk.settings["cameraMaxDistance"] = 25f;
        }

        protected override void onUpdate() {
            var camera = (ActiveObject) sdk.refs.mapObjects.retrieve("CameraPreset_TopDownFollow");
            
            sdk.refs.client.SetState(camera.ObjectId, "MaxZoom", (float) sdk.settings["cameraMaxDistance"]);
        }

        protected override void onStop() {
            sdk.settings["cameraMaxDistance"] = 25f;
        }
    }
}
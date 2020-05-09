namespace AlphaRite.sdk {
    public abstract class AlphaCycle {
        private bool _enabled;
        protected AlphariteSdk sdk;
        
        public AlphaCycle(AlphariteSdk sdk) {
            this._enabled = false;
            this.sdk = sdk;
        }

        protected abstract void onStart();
        protected abstract void onStop();
        protected virtual void onUpdate() {}
        protected virtual void onRenderingUpdate() {}
        
        public void enable() {
            if (_enabled) return;
            onStart();
            _enabled = true;
        }

        public void disable() {
            if (!_enabled) return;
            onStop();
            _enabled = false;
        }

        public void update() {
            if (!_enabled) return;
            onUpdate();
        }

        public void renderingUpdate() {
            if (!_enabled) return;
            onRenderingUpdate();
        }
    }
}
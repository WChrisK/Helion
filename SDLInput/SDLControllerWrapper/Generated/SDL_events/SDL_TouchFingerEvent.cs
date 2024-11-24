namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_TouchFingerEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_TouchID")]
        public long touchId;

        [NativeTypeName("SDL_FingerID")]
        public long fingerId;

        public float x;

        public float y;

        public float dx;

        public float dy;

        public float pressure;

        [NativeTypeName("Uint32")]
        public uint windowID;
    }
}

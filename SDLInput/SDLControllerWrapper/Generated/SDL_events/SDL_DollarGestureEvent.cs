namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_DollarGestureEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_TouchID")]
        public long touchId;

        [NativeTypeName("SDL_GestureID")]
        public long gestureId;

        [NativeTypeName("Uint32")]
        public uint numFingers;

        public float error;

        public float x;

        public float y;
    }
}

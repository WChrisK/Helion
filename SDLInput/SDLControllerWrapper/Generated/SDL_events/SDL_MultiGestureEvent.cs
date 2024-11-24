namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;

    public partial struct SDL_MultiGestureEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_TouchID")]
        public long touchId;

        public float dTheta;

        public float dDist;

        public float x;

        public float y;

        [NativeTypeName("Uint16")]
        public ushort numFingers;

        [NativeTypeName("Uint16")]
        public ushort padding;
    }
}

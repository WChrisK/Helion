namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;

    public unsafe partial struct SDL_TextInputEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("Uint32")]
        public uint windowID;

        [NativeTypeName("char[32]")]
        public fixed sbyte text[32];
    }
}

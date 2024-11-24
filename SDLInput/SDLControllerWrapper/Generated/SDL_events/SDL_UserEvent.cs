namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public unsafe partial struct SDL_UserEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("Uint32")]
        public uint windowID;

        [NativeTypeName("Sint32")]
        public int code;

        public void* data1;

        public void* data2;
    }
}

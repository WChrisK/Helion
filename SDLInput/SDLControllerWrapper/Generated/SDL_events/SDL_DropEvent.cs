namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public unsafe partial struct SDL_DropEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("char *")]
        public sbyte* file;

        [NativeTypeName("Uint32")]
        public uint windowID;
    }
}

namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_MouseMotionEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("Uint32")]
        public uint windowID;

        [NativeTypeName("Uint32")]
        public uint which;

        [NativeTypeName("Uint32")]
        public uint state;

        [NativeTypeName("Sint32")]
        public int x;

        [NativeTypeName("Sint32")]
        public int y;

        [NativeTypeName("Sint32")]
        public int xrel;

        [NativeTypeName("Sint32")]
        public int yrel;
    }
}

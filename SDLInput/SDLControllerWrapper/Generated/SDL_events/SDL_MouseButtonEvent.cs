namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_MouseButtonEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("Uint32")]
        public uint windowID;

        [NativeTypeName("Uint32")]
        public uint which;

        [NativeTypeName("Uint8")]
        public byte button;

        [NativeTypeName("Uint8")]
        public byte state;

        [NativeTypeName("Uint8")]
        public byte clicks;

        [NativeTypeName("Uint8")]
        public byte padding1;

        [NativeTypeName("Sint32")]
        public int x;

        [NativeTypeName("Sint32")]
        public int y;
    }
}

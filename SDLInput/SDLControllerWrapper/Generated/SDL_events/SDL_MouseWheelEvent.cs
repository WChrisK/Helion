namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_MouseWheelEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("Uint32")]
        public uint windowID;

        [NativeTypeName("Uint32")]
        public uint which;

        [NativeTypeName("Sint32")]
        public int x;

        [NativeTypeName("Sint32")]
        public int y;

        [NativeTypeName("Uint32")]
        public uint direction;

        public float preciseX;

        public float preciseY;

        [NativeTypeName("Sint32")]
        public int mouseX;

        [NativeTypeName("Sint32")]
        public int mouseY;
    }
}

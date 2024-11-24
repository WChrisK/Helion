namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;

    public partial struct SDL_WindowEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("Uint32")]
        public uint windowID;

        [NativeTypeName("Uint8")]
        public byte @event;

        [NativeTypeName("Uint8")]
        public byte padding1;

        [NativeTypeName("Uint8")]
        public byte padding2;

        [NativeTypeName("Uint8")]
        public byte padding3;

        [NativeTypeName("Sint32")]
        public int data1;

        [NativeTypeName("Sint32")]
        public int data2;
    }
}

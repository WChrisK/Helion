namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;

    public partial struct SDL_JoyButtonEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_JoystickID")]
        public int which;

        [NativeTypeName("Uint8")]
        public byte button;

        [NativeTypeName("Uint8")]
        public byte state;

        [NativeTypeName("Uint8")]
        public byte padding1;

        [NativeTypeName("Uint8")]
        public byte padding2;
    }
}

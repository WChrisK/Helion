namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_JoyHatEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_JoystickID")]
        public int which;

        [NativeTypeName("Uint8")]
        public byte hat;

        [NativeTypeName("Uint8")]
        public byte value;

        [NativeTypeName("Uint8")]
        public byte padding1;

        [NativeTypeName("Uint8")]
        public byte padding2;
    }
}

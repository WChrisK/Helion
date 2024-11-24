namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;

    public partial struct SDL_JoyBallEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_JoystickID")]
        public int which;

        [NativeTypeName("Uint8")]
        public byte ball;

        [NativeTypeName("Uint8")]
        public byte padding1;

        [NativeTypeName("Uint8")]
        public byte padding2;

        [NativeTypeName("Uint8")]
        public byte padding3;

        [NativeTypeName("Sint16")]
        public short xrel;

        [NativeTypeName("Sint16")]
        public short yrel;
    }
}

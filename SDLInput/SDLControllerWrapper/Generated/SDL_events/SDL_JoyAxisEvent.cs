namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_JoyAxisEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_JoystickID")]
        public int which;

        [NativeTypeName("Uint8")]
        public byte axis;

        [NativeTypeName("Uint8")]
        public byte padding1;

        [NativeTypeName("Uint8")]
        public byte padding2;

        [NativeTypeName("Uint8")]
        public byte padding3;

        [NativeTypeName("Sint16")]
        public short value;

        [NativeTypeName("Uint16")]
        public ushort padding4;
    }
}

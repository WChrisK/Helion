namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    public partial struct SDL_AudioDeviceEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("Uint32")]
        public uint which;

        [NativeTypeName("Uint8")]
        public byte iscapture;

        [NativeTypeName("Uint8")]
        public byte padding1;

        [NativeTypeName("Uint8")]
        public byte padding2;

        [NativeTypeName("Uint8")]
        public byte padding3;
    }
}

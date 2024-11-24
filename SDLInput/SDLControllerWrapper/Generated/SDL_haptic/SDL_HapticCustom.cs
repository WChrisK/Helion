namespace SDLControllerWrapper.Generated.SDL_haptic
{
    using global::SDLControllerWrapper.Generated.Shared;
    public unsafe partial struct SDL_HapticCustom
    {
        [NativeTypeName("Uint16")]
        public ushort type;

        public SDL_HapticDirection direction;

        [NativeTypeName("Uint32")]
        public uint length;

        [NativeTypeName("Uint16")]
        public ushort delay;

        [NativeTypeName("Uint16")]
        public ushort button;

        [NativeTypeName("Uint16")]
        public ushort interval;

        [NativeTypeName("Uint8")]
        public byte channels;

        [NativeTypeName("Uint16")]
        public ushort period;

        [NativeTypeName("Uint16")]
        public ushort samples;

        [NativeTypeName("Uint16 *")]
        public ushort* data;

        [NativeTypeName("Uint16")]
        public ushort attack_length;

        [NativeTypeName("Uint16")]
        public ushort attack_level;

        [NativeTypeName("Uint16")]
        public ushort fade_length;

        [NativeTypeName("Uint16")]
        public ushort fade_level;
    }
}

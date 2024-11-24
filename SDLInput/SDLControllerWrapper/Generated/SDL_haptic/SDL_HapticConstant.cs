namespace SDLControllerWrapper.Generated.SDL_haptic
{
    using global::SDLControllerWrapper.Generated.Shared;

    public partial struct SDL_HapticConstant
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

        [NativeTypeName("Sint16")]
        public short level;

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

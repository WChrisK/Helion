namespace SDLControllerWrapper.Generated.SDL_haptic
{
    using global::SDLControllerWrapper.Generated.Shared;
    public unsafe partial struct SDL_HapticCondition
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

        [NativeTypeName("Uint16[3]")]
        public fixed ushort right_sat[3];

        [NativeTypeName("Uint16[3]")]
        public fixed ushort left_sat[3];

        [NativeTypeName("Sint16[3]")]
        public fixed short right_coeff[3];

        [NativeTypeName("Sint16[3]")]
        public fixed short left_coeff[3];

        [NativeTypeName("Uint16[3]")]
        public fixed ushort deadband[3];

        [NativeTypeName("Sint16[3]")]
        public fixed short center[3];
    }
}

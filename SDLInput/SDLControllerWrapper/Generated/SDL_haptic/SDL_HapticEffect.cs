namespace SDLControllerWrapper.Generated.SDL_haptic
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    public partial struct SDL_HapticEffect
    {
        [FieldOffset(0)]
        [NativeTypeName("Uint16")]
        public ushort type;

        [FieldOffset(0)]
        public SDL_HapticConstant constant;

        [FieldOffset(0)]
        public SDL_HapticPeriodic periodic;

        [FieldOffset(0)]
        public SDL_HapticCondition condition;

        [FieldOffset(0)]
        public SDL_HapticRamp ramp;

        [FieldOffset(0)]
        public SDL_HapticLeftRight leftright;

        [FieldOffset(0)]
        public SDL_HapticCustom custom;
    }
}

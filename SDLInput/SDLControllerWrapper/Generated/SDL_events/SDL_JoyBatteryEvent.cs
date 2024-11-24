namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.SDL_joystick;
    using global::SDLControllerWrapper.Generated.Shared;

    public partial struct SDL_JoyBatteryEvent
    {
        [NativeTypeName("Uint32")]
        public uint type;

        [NativeTypeName("Uint32")]
        public uint timestamp;

        [NativeTypeName("SDL_JoystickID")]
        public int which;

        public SDL_JoystickPowerLevel level;
    }
}

namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct SDL_Event
    {
        [FieldOffset(0)]
        [NativeTypeName("Uint32")]
        public uint type;

        [FieldOffset(0)]
        public SDL_CommonEvent common;

        [FieldOffset(0)]
        public SDL_DisplayEvent display;

        [FieldOffset(0)]
        public SDL_WindowEvent window;

        [FieldOffset(0)]
        public SDL_KeyboardEvent key;

        [FieldOffset(0)]
        public SDL_TextEditingEvent edit;

        [FieldOffset(0)]
        public SDL_TextEditingExtEvent editExt;

        [FieldOffset(0)]
        public SDL_TextInputEvent text;

        [FieldOffset(0)]
        public SDL_MouseMotionEvent motion;

        [FieldOffset(0)]
        public SDL_MouseButtonEvent button;

        [FieldOffset(0)]
        public SDL_MouseWheelEvent wheel;

        [FieldOffset(0)]
        public SDL_JoyAxisEvent jaxis;

        [FieldOffset(0)]
        public SDL_JoyBallEvent jball;

        [FieldOffset(0)]
        public SDL_JoyHatEvent jhat;

        [FieldOffset(0)]
        public SDL_JoyButtonEvent jbutton;

        [FieldOffset(0)]
        public SDL_JoyDeviceEvent jdevice;

        [FieldOffset(0)]
        public SDL_JoyBatteryEvent jbattery;

        [FieldOffset(0)]
        public SDL_ControllerAxisEvent caxis;

        [FieldOffset(0)]
        public SDL_ControllerButtonEvent cbutton;

        [FieldOffset(0)]
        public SDL_ControllerDeviceEvent cdevice;

        [FieldOffset(0)]
        public SDL_ControllerTouchpadEvent ctouchpad;

        [FieldOffset(0)]
        public SDL_ControllerSensorEvent csensor;

        [FieldOffset(0)]
        public SDL_AudioDeviceEvent adevice;

        [FieldOffset(0)]
        public SDL_SensorEvent sensor;

        [FieldOffset(0)]
        public SDL_QuitEvent quit;

        [FieldOffset(0)]
        public SDL_UserEvent user;

        [FieldOffset(0)]
        public SDL_SysWMEvent syswm;

        [FieldOffset(0)]
        public SDL_TouchFingerEvent tfinger;

        [FieldOffset(0)]
        public SDL_MultiGestureEvent mgesture;

        [FieldOffset(0)]
        public SDL_DollarGestureEvent dgesture;

        [FieldOffset(0)]
        public SDL_DropEvent drop;

        [FieldOffset(0)]
        [NativeTypeName("Uint8[56]")]
        public fixed byte padding[56];
    }
}

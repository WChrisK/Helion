namespace SDLControllerWrapper.Generated.SDL_joystick
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public static unsafe partial class SDL_joystick
    {
        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_LockJoysticks();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_UnlockJoysticks();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_NumJoysticks();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_JoystickNameForIndex(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_JoystickPathForIndex(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickGetDevicePlayerIndex(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_JoystickGUID")]
        public static extern System.Guid SDL_JoystickGetDeviceGUID(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_JoystickGetDeviceVendor(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_JoystickGetDeviceProduct(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_JoystickGetDeviceProductVersion(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_JoystickType SDL_JoystickGetDeviceType(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_JoystickID")]
        public static extern int SDL_JoystickGetDeviceInstanceID(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Joystick *")]
        public static extern _SDL_Joystick* SDL_JoystickOpen(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Joystick *")]
        public static extern _SDL_Joystick* SDL_JoystickFromInstanceID([NativeTypeName("SDL_JoystickID")] int instance_id);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Joystick *")]
        public static extern _SDL_Joystick* SDL_JoystickFromPlayerIndex(int player_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickAttachVirtual(SDL_JoystickType type, int naxes, int nbuttons, int nhats);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickAttachVirtualEx([NativeTypeName("const SDL_VirtualJoystickDesc *")] SDL_VirtualJoystickDesc* desc);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickDetachVirtual(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_JoystickIsVirtual(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickSetVirtualAxis([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int axis, [NativeTypeName("Sint16")] short value);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickSetVirtualButton([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int button, [NativeTypeName("Uint8")] byte value);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickSetVirtualHat([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int hat, [NativeTypeName("Uint8")] byte value);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_JoystickName([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_JoystickPath([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickGetPlayerIndex([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_JoystickSetPlayerIndex([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int player_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_JoystickGUID")]
        public static extern System.Guid SDL_JoystickGetGUID([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_JoystickGetVendor([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_JoystickGetProduct([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_JoystickGetProductVersion([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_JoystickGetFirmwareVersion([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_JoystickGetSerial([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_JoystickType SDL_JoystickGetType([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_JoystickGetGUIDString([NativeTypeName("SDL_JoystickGUID")] System.Guid guid, [NativeTypeName("char *")] sbyte* pszGUID, int cbGUID);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_JoystickGUID")]
        public static extern System.Guid SDL_JoystickGetGUIDFromString([NativeTypeName("const char *")] sbyte* pchGUID);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_GetJoystickGUIDInfo([NativeTypeName("SDL_JoystickGUID")] System.Guid guid, [NativeTypeName("Uint16 *")] ushort* vendor, [NativeTypeName("Uint16 *")] ushort* product, [NativeTypeName("Uint16 *")] ushort* version, [NativeTypeName("Uint16 *")] ushort* crc16);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_JoystickGetAttached([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_JoystickID")]
        public static extern int SDL_JoystickInstanceID([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickNumAxes([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickNumBalls([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickNumHats([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickNumButtons([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_JoystickUpdate();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickEventState(int state);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Sint16")]
        public static extern short SDL_JoystickGetAxis([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int axis);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_JoystickGetAxisInitialState([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int axis, [NativeTypeName("Sint16 *")] short* state);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint8")]
        public static extern byte SDL_JoystickGetHat([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int hat);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickGetBall([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int ball, int* dx, int* dy);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint8")]
        public static extern byte SDL_JoystickGetButton([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, int button);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickRumble([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, [NativeTypeName("Uint16")] ushort low_frequency_rumble, [NativeTypeName("Uint16")] ushort high_frequency_rumble, [NativeTypeName("Uint32")] uint duration_ms);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickRumbleTriggers([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, [NativeTypeName("Uint16")] ushort left_rumble, [NativeTypeName("Uint16")] ushort right_rumble, [NativeTypeName("Uint32")] uint duration_ms);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_JoystickHasLED([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_JoystickHasRumble([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_JoystickHasRumbleTriggers([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickSetLED([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, [NativeTypeName("Uint8")] byte red, [NativeTypeName("Uint8")] byte green, [NativeTypeName("Uint8")] byte blue);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickSendEffect([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick, [NativeTypeName("const void *")] void* data, int size);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_JoystickClose([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_JoystickPowerLevel SDL_JoystickCurrentPowerLevel([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);
    }
}

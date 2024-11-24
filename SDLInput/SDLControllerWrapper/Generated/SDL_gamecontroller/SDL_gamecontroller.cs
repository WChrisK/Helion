namespace SDLControllerWrapper.Generated.SDL_gamecontroller
{
    using global::SDLControllerWrapper.Generated.SDL_joystick;
    using global::SDLControllerWrapper.Generated.SDL_sensor;
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public static unsafe partial class SDL_gamecontroller
    {
        //[DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        //public static extern int SDL_GameControllerAddMappingsFromRW(SDL_RWops* rw, int freerw);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerAddMapping([NativeTypeName("const char *")] sbyte* mappingString);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerNumMappings();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* SDL_GameControllerMappingForIndex(int mapping_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* SDL_GameControllerMappingForGUID([NativeTypeName("SDL_JoystickGUID")] System.Guid guid);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* SDL_GameControllerMapping([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_IsGameController(int joystick_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerNameForIndex(int joystick_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerPathForIndex(int joystick_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_GameControllerType SDL_GameControllerTypeForIndex(int joystick_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* SDL_GameControllerMappingForDeviceIndex(int joystick_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_GameController *")]
        public static extern _SDL_GameController* SDL_GameControllerOpen(int joystick_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_GameController *")]
        public static extern _SDL_GameController* SDL_GameControllerFromInstanceID([NativeTypeName("SDL_JoystickID")] int joyid);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_GameController *")]
        public static extern _SDL_GameController* SDL_GameControllerFromPlayerIndex(int player_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerName([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerPath([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_GameControllerType SDL_GameControllerGetType([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerGetPlayerIndex([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_GameControllerSetPlayerIndex([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, int player_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_GameControllerGetVendor([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_GameControllerGetProduct([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_GameControllerGetProductVersion([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint16")]
        public static extern ushort SDL_GameControllerGetFirmwareVersion([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerGetSerial([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint64")]
        public static extern ulong SDL_GameControllerGetSteamHandle([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerGetAttached([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Joystick *")]
        public static extern _SDL_Joystick* SDL_GameControllerGetJoystick([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerEventState(int state);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_GameControllerUpdate();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_GameControllerAxis SDL_GameControllerGetAxisFromString([NativeTypeName("const char *")] sbyte* str);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerGetStringForAxis(SDL_GameControllerAxis axis);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_GameControllerButtonBind SDL_GameControllerGetBindForAxis([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerAxis axis);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerHasAxis([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerAxis axis);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Sint16")]
        public static extern short SDL_GameControllerGetAxis([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerAxis axis);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_GameControllerButton SDL_GameControllerGetButtonFromString([NativeTypeName("const char *")] sbyte* str);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerGetStringForButton(SDL_GameControllerButton button);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_GameControllerButtonBind SDL_GameControllerGetBindForButton([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerButton button);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerHasButton([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerButton button);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint8")]
        public static extern byte SDL_GameControllerGetButton([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerButton button);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerGetNumTouchpads([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerGetNumTouchpadFingers([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, int touchpad);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerGetTouchpadFinger([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, int touchpad, int finger, [NativeTypeName("Uint8 *")] byte* state, float* x, float* y, float* pressure);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerHasSensor([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_SensorType type);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerSetSensorEnabled([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_SensorType type, SDL_bool enabled);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerIsSensorEnabled([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_SensorType type);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern float SDL_GameControllerGetSensorDataRate([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_SensorType type);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerGetSensorData([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_SensorType type, float* data, int num_values);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerGetSensorDataWithTimestamp([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_SensorType type, [NativeTypeName("Uint64 *")] ulong* timestamp, float* data, int num_values);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerRumble([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, [NativeTypeName("Uint16")] ushort low_frequency_rumble, [NativeTypeName("Uint16")] ushort high_frequency_rumble, [NativeTypeName("Uint32")] uint duration_ms);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerRumbleTriggers([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, [NativeTypeName("Uint16")] ushort left_rumble, [NativeTypeName("Uint16")] ushort right_rumble, [NativeTypeName("Uint32")] uint duration_ms);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerHasLED([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerHasRumble([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GameControllerHasRumbleTriggers([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerSetLED([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, [NativeTypeName("Uint8")] byte red, [NativeTypeName("Uint8")] byte green, [NativeTypeName("Uint8")] byte blue);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_GameControllerSendEffect([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, [NativeTypeName("const void *")] void* data, int size);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_GameControllerClose([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerGetAppleSFSymbolsNameForButton([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerButton button);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GameControllerGetAppleSFSymbolsNameForAxis([NativeTypeName("SDL_GameController *")] _SDL_GameController* gamecontroller, SDL_GameControllerAxis axis);
    }
}

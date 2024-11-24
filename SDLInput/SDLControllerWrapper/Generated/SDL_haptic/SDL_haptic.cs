namespace SDLControllerWrapper.Generated.SDL_haptic
{
    using global::SDLControllerWrapper.Generated.SDL_joystick;
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public static unsafe partial class SDL_haptic
    {
        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_NumHaptics();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_HapticName(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Haptic *")]
        public static extern _SDL_Haptic* SDL_HapticOpen(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticOpened(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticIndex([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_MouseIsHaptic();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Haptic *")]
        public static extern _SDL_Haptic* SDL_HapticOpenFromMouse();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_JoystickIsHaptic([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Haptic *")]
        public static extern _SDL_Haptic* SDL_HapticOpenFromJoystick([NativeTypeName("SDL_Joystick *")] _SDL_Joystick* joystick);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_HapticClose([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticNumEffects([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticNumEffectsPlaying([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint SDL_HapticQuery([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticNumAxes([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticEffectSupported([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, SDL_HapticEffect* effect);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticNewEffect([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, SDL_HapticEffect* effect);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticUpdateEffect([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, int effect, SDL_HapticEffect* data);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticRunEffect([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, int effect, [NativeTypeName("Uint32")] uint iterations);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticStopEffect([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, int effect);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_HapticDestroyEffect([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, int effect);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticGetEffectStatus([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, int effect);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticSetGain([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, int gain);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticSetAutocenter([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, int autocenter);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticPause([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticUnpause([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticStopAll([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticRumbleSupported([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticRumbleInit([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticRumblePlay([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic, float strength, [NativeTypeName("Uint32")] uint length);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_HapticRumbleStop([NativeTypeName("SDL_Haptic *")] _SDL_Haptic* haptic);
    }
}

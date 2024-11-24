namespace SDLControllerWrapper.Generated.SDL
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System;
    using System.Runtime.InteropServices;

    public static partial class SDL
    {
        [Flags]
        public enum InitFlags : uint
        {
            SDL_INIT_TIMER = 0x00000001u,
            SDL_INIT_AUDIO = 0x00000010u,
            SDL_INIT_VIDEO = 0x00000020u,
            SDL_INIT_JOYSTICK = 0x00000200u,
            SDL_INIT_HAPTIC = 0x00001000u,
            SDL_INIT_GAMECONTROLLER = 0x00002000u,
            SDL_INIT_EVENTS = 0x00004000u,
            SDL_INIT_SENSOR = 0x00008000u,
            SDL_INIT_NOPARACHUTE = 0x00100000u,
            SDL_INIT_EVERYTHING = SDL_INIT_TIMER | SDL_INIT_AUDIO | SDL_INIT_VIDEO | SDL_INIT_EVENTS
                | SDL_INIT_JOYSTICK | SDL_INIT_HAPTIC | SDL_INIT_GAMECONTROLLER | SDL_INIT_SENSOR
        };


        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_Init([NativeTypeName("Uint32")] InitFlags flags);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_InitSubSystem([NativeTypeName("Uint32")] InitFlags flags);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_QuitSubSystem([NativeTypeName("Uint32")] InitFlags flags);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint32")]
        public static extern uint SDL_WasInit([NativeTypeName("Uint32")] InitFlags flags);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_Quit();
    }
}
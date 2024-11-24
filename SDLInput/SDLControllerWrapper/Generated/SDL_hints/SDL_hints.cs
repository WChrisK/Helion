namespace SDLControllerWrapper.Generated.SDL_hints
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public static unsafe partial class SDL_hints
    {
        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_SetHintWithPriority([NativeTypeName("const char *")] sbyte* name, [NativeTypeName("const char *")] sbyte* value, SDL_HintPriority priority);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_SetHint([NativeTypeName("const char *")] sbyte* name, [NativeTypeName("const char *")] sbyte* value);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_ResetHint([NativeTypeName("const char *")] sbyte* name);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_ResetHints();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_GetHint([NativeTypeName("const char *")] sbyte* name);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GetHintBoolean([NativeTypeName("const char *")] sbyte* name, SDL_bool default_value);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_AddHintCallback([NativeTypeName("const char *")] sbyte* name, [NativeTypeName("SDL_HintCallback")] delegate* unmanaged[Cdecl]<void*, sbyte*, sbyte*, sbyte*, void> callback, void* userdata);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_DelHintCallback([NativeTypeName("const char *")] sbyte* name, [NativeTypeName("SDL_HintCallback")] delegate* unmanaged[Cdecl]<void*, sbyte*, sbyte*, sbyte*, void> callback, void* userdata);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_ClearHints();
    }
}

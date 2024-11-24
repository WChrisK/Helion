namespace SDLControllerWrapper.Generated.SDL_events
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public static unsafe partial class SDL_Events
    {
        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_PumpEvents();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_PeepEvents(SDL_Event* events, int numevents, SDL_eventaction action, [NativeTypeName("Uint32")] uint minType, [NativeTypeName("Uint32")] uint maxType);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_HasEvent([NativeTypeName("Uint32")] uint type);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_HasEvents([NativeTypeName("Uint32")] uint minType, [NativeTypeName("Uint32")] uint maxType);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_FlushEvent([NativeTypeName("Uint32")] uint type);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_FlushEvents([NativeTypeName("Uint32")] uint minType, [NativeTypeName("Uint32")] uint maxType);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_PollEvent(SDL_Event* @event);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_WaitEvent(SDL_Event* @event);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_WaitEventTimeout(SDL_Event* @event, int timeout);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_PushEvent(SDL_Event* @event);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_SetEventFilter([NativeTypeName("SDL_EventFilter")] delegate* unmanaged[Cdecl]<void*, SDL_Event*, int> filter, void* userdata);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_bool SDL_GetEventFilter([NativeTypeName("SDL_EventFilter *")] delegate* unmanaged[Cdecl]<void*, SDL_Event*, int>* filter, void** userdata);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_AddEventWatch([NativeTypeName("SDL_EventFilter")] delegate* unmanaged[Cdecl]<void*, SDL_Event*, int> filter, void* userdata);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_DelEventWatch([NativeTypeName("SDL_EventFilter")] delegate* unmanaged[Cdecl]<void*, SDL_Event*, int> filter, void* userdata);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_FilterEvents([NativeTypeName("SDL_EventFilter")] delegate* unmanaged[Cdecl]<void*, SDL_Event*, int> filter, void* userdata);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint8")]
        public static extern byte SDL_EventState([NativeTypeName("Uint32")] uint type, int state);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint32")]
        public static extern uint SDL_RegisterEvents(int numevents);
    }
}

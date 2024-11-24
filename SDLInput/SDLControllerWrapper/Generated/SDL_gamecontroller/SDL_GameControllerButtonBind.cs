namespace SDLControllerWrapper.Generated.SDL_gamecontroller
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public partial struct SDL_GameControllerButtonBind
    {
        public SDL_GameControllerBindType bindType;

        [NativeTypeName("__AnonymousRecord_SDL_gamecontroller_L94_C5")]
        public _value_e__Union value;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _value_e__Union
        {
            [FieldOffset(0)]
            public int button;

            [FieldOffset(0)]
            public int axis;

            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_SDL_gamecontroller_L98_C9")]
            public _hat_e__Struct hat;

            public partial struct _hat_e__Struct
            {
                public int hat;

                public int hat_mask;
            }
        }
    }
}

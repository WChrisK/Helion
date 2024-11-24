namespace SDLControllerWrapper.Generated.SDL_hidapi
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public static unsafe partial class SDL_hidapi
    {
        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_init();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_exit();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("Uint32")]
        public static extern uint SDL_hid_device_change_count();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_hid_device_info* SDL_hid_enumerate([NativeTypeName("unsigned short")] ushort vendor_id, [NativeTypeName("unsigned short")] ushort product_id);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_hid_free_enumeration(SDL_hid_device_info* devs);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_hid_device *")]
        public static extern SDL_hid_device_* SDL_hid_open([NativeTypeName("unsigned short")] ushort vendor_id, [NativeTypeName("unsigned short")] ushort product_id, [NativeTypeName("const wchar_t *")] ushort* serial_number);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_hid_device *")]
        public static extern SDL_hid_device_* SDL_hid_open_path([NativeTypeName("const char *")] sbyte* path, int bExclusive);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_write([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("const unsigned char *")] byte* data, [NativeTypeName("size_t")] nuint length);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_read_timeout([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("unsigned char *")] byte* data, [NativeTypeName("size_t")] nuint length, int milliseconds);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_read([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("unsigned char *")] byte* data, [NativeTypeName("size_t")] nuint length);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_set_nonblocking([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, int nonblock);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_send_feature_report([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("const unsigned char *")] byte* data, [NativeTypeName("size_t")] nuint length);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_get_feature_report([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("unsigned char *")] byte* data, [NativeTypeName("size_t")] nuint length);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_hid_close([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_get_manufacturer_string([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("wchar_t *")] ushort* @string, [NativeTypeName("size_t")] nuint maxlen);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_get_product_string([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("wchar_t *")] ushort* @string, [NativeTypeName("size_t")] nuint maxlen);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_get_serial_number_string([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, [NativeTypeName("wchar_t *")] ushort* @string, [NativeTypeName("size_t")] nuint maxlen);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_hid_get_indexed_string([NativeTypeName("SDL_hid_device *")] SDL_hid_device_* dev, int string_index, [NativeTypeName("wchar_t *")] ushort* @string, [NativeTypeName("size_t")] nuint maxlen);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_hid_ble_scan(SDL_bool active);
    }
}

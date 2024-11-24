namespace SDLControllerWrapper.Generated.SDL_sensor
{
    using global::SDLControllerWrapper.Generated.Shared;
    using System.Runtime.InteropServices;

    public static unsafe partial class SDL_sensor
    {
        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_LockSensors();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_UnlockSensors();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_NumSensors();

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_SensorGetDeviceName(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_SensorType SDL_SensorGetDeviceType(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_SensorGetDeviceNonPortableType(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_SensorID")]
        public static extern int SDL_SensorGetDeviceInstanceID(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Sensor *")]
        public static extern _SDL_Sensor* SDL_SensorOpen(int device_index);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_Sensor *")]
        public static extern _SDL_Sensor* SDL_SensorFromInstanceID([NativeTypeName("SDL_SensorID")] int instance_id);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* SDL_SensorGetName([NativeTypeName("SDL_Sensor *")] _SDL_Sensor* sensor);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SDL_SensorType SDL_SensorGetType([NativeTypeName("SDL_Sensor *")] _SDL_Sensor* sensor);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_SensorGetNonPortableType([NativeTypeName("SDL_Sensor *")] _SDL_Sensor* sensor);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("SDL_SensorID")]
        public static extern int SDL_SensorGetInstanceID([NativeTypeName("SDL_Sensor *")] _SDL_Sensor* sensor);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_SensorGetData([NativeTypeName("SDL_Sensor *")] _SDL_Sensor* sensor, float* data, int num_values);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int SDL_SensorGetDataWithTimestamp([NativeTypeName("SDL_Sensor *")] _SDL_Sensor* sensor, [NativeTypeName("Uint64 *")] ulong* timestamp, float* data, int num_values);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_SensorClose([NativeTypeName("SDL_Sensor *")] _SDL_Sensor* sensor);

        [DllImport(SDLControllerWrapper.LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void SDL_SensorUpdate();
    }
}

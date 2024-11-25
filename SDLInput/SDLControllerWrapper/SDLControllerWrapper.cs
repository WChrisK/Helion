namespace SDLControllerWrapper
{
    using Generated.SDL;
    using Generated.SDL_events;
    using Generated.SDL_gamecontroller;
    using Generated.SDL_hints;
    using Generated.SDL_joystick;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public partial class SDLControllerWrapper : IDisposable
    {
        private const string CONTROLLERMAPPINGSDB = "SDLControllerWrapper.Resources.GameControllerDB.txt";
        private const SDL.InitFlags INITFLAGS = SDL.InitFlags.SDL_INIT_JOYSTICK | SDL.InitFlags.SDL_INIT_GAMECONTROLLER | SDL.InitFlags.SDL_INIT_SENSOR | SDL.InitFlags.SDL_INIT_EVENTS;

        private bool _disposedValue;
        private static SDLControllerWrapper? _instance;
        private readonly FilterDelegate _controllerEventFilter;

        private event EventHandler<ConfigurationEvent> ConfigurationChanged;

        private List<Controller> _controllers;

        /// <summary>
        /// Gets the singleton controller wrapper (if initialized)
        /// </summary>
        public static SDLControllerWrapper? Instance => _instance;

        /// <summary>
        /// Gets the set of <see cref="Controllers"/> connected to the system
        /// </summary>
        public ReadOnlyCollection<Controller> Controllers => this._controllers.AsReadOnly();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void FilterDelegate(void* userData, SDL_Event* evt, int retVal);

        /// <summary>
        /// Initialize a controller wrapper
        /// </summary>
        /// <param name="controllerChangeHandler">This function will be called when a controller is connected or disconnected.</param>
        /// <exception cref="InvalidOperationException">Thrown if there is already a controller wrapper active</exception>        
        public unsafe SDLControllerWrapper(EventHandler<ConfigurationEvent> controllerChangeHandler)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Can only have one ControllerWrapper at a time");
            }

            // Ask SDL to send us events when we call Update()
            SetHint(SDL_HintConstants.SDL_HINT_AUTO_UPDATE_JOYSTICKS, "1");
            SetHint(SDL_HintConstants.SDL_HINT_JOYSTICK_HIDAPI, "1");

            // Disable RawInput, or XBox controller rumble is broken.
            //https://github.com/libsdl-org/SDL/issues/4072
            SetHint(SDL_HintConstants.SDL_HINT_JOYSTICK_RAWINPUT, "0");

            // Enable rumble on PS4/PS5 controllers via HID API (disabled by default)
            SetHint(SDL_HintConstants.SDL_HINT_JOYSTICK_HIDAPI_PS4_RUMBLE, "1");
            SetHint(SDL_HintConstants.SDL_HINT_JOYSTICK_HIDAPI_PS5_RUMBLE, "1");

            // Initialize SDL and load the  mapping DB
            _ = SDL.SDL_Init(INITFLAGS);
            LoadMappingDatabase();

            // Detect initial set of controllers
            this._controllers = [];
            this.DetectControllers();

            // Configure a callback to handle events from SDL
            this._controllerEventFilter = new FilterDelegate(this.ControllerEventFilter);
            nint filterPtr = Marshal.GetFunctionPointerForDelegate(this._controllerEventFilter);
            SDL_Events.SDL_AddEventWatch((delegate* unmanaged[Cdecl]<void*, SDL_Event*, int>)filterPtr, null);

            ConfigurationChanged += controllerChangeHandler;
            _instance = this;
        }

        public void Poll()
        {
            SDL_gamecontroller.SDL_GameControllerUpdate();
            foreach (Controller controller in this.Controllers)
            {
                controller.Poll();
            }
        }

        /// <summary>
        /// Redetect connected controllers
        /// It should only be necessary to call this if there's been a very long gap since the last polling event
        /// (in which case, the event buffer might have overrun, causing us to lose a connect/disconnect event).
        /// </summary>
        public void DetectControllers()
        {
            int numJoysticks = SDL_joystick.SDL_NumJoysticks();
            for (int i = 0; i < numJoysticks; i++)
            {
                if (Controller.IsController(i))
                {
                    this._controllers.Add(new Controller(i));
                }
            }
        }

        private unsafe void ControllerEventFilter(void* userData, SDL_Event* evt, int retVal)
        {
            SDL_Event unwrapped = *evt;

            switch ((SDL_EventType)unwrapped.type)
            {
                case SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                    this._controllers.Add(new Controller(unwrapped.cdevice.which));
                    ConfigurationChanged(this, new(ConfigurationChange.Added, unwrapped.cdevice.which));
                    break;
                case SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                    ConfigurationChanged(this, new(ConfigurationChange.Removed, unwrapped.cdevice.which));
                    _ = this._controllers.RemoveAll(c => c.JoystickIndex == unwrapped.cdevice.which);
                    break;
                default:
                    break;
            }
        }

        private static unsafe void LoadMappingDatabase()
        {
            // Load button mappings for controllers
            using (StreamReader reader = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(CONTROLLERMAPPINGSDB)!))
            {
                string? line = reader.ReadLine();
                while (line != null)
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {
                        byte[] mappingBytes = Encoding.ASCII.GetBytes(line);
                        fixed (byte* p = mappingBytes)
                        {
                            _ = SDL_gamecontroller.SDL_GameControllerAddMapping((sbyte*)p);
                        }
                    }

                    line = reader.ReadLine();
                }
            }
        }

        private static unsafe void SetHint(string name, string value)
        {
            byte[] nameBytes = Encoding.ASCII.GetBytes(name);
            byte[] valueBytes = Encoding.ASCII.GetBytes(value);

            unsafe
            {
                fixed (byte* p = nameBytes, q = valueBytes)
                {
                    _ = SDL_hints.SDL_SetHint((sbyte*)p, (sbyte*)q);
                }
            }
        }

        protected virtual unsafe void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    _instance = null;

                    foreach (Delegate del in ConfigurationChanged.GetInvocationList())
                    {
                        ConfigurationChanged -= (EventHandler<ConfigurationEvent>)del;
                    }
                }

                foreach (Controller ctrl in this._controllers)
                {
                    ctrl.Dispose();
                }
                this._controllers.Clear();

                // Unhook controller event handler
                nint filterPtr = Marshal.GetFunctionPointerForDelegate(this._controllerEventFilter);
                SDL_Events.SDL_DelEventWatch((delegate* unmanaged[Cdecl]<void*, SDL_Event*, int>)filterPtr, null);

                // Let go of SDL
                SDL.SDL_QuitSubSystem(INITFLAGS);
                this._disposedValue = true;
            }
        }

        ~SDLControllerWrapper()
        {
            this.Dispose(disposing: false);
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

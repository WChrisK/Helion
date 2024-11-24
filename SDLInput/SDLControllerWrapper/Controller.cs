namespace SDLControllerWrapper
{
    using Generated.SDL_gamecontroller;
    using global::SDLControllerWrapper.Generated.SDL_sensor;
    using System;

    public unsafe class Controller : IDisposable
    {
        private bool _disposedValue;
        internal _SDL_GameController* _controller;

        /// <summary>
        /// The <see cref="Joystick"/> this game controller is correlated with
        /// </summary>
        public readonly int JoystickIndex;

        /// <summary>
        /// Whether the controller supports rumble effects
        /// </summary>
        public readonly bool HasRumble;

        /// <summary>
        /// Whether the game controller has a gyroscope sensor
        /// </summary>
        public readonly bool HasGyro;

        /// <summary>
        /// Whether the game controller has an accelerometer
        /// </summary>
        public readonly bool HasAccel;

        /// <summary>
        /// The name of the controller according to SDL
        /// </summary>
        public readonly string Name;

        private readonly bool[][] _buttonStates;
        private readonly float[][] _axisStates;
        private readonly DPad[] _dpadStates;
        private readonly float[][] _gyroStates;
        private readonly float[][] _accelStates;

        private int _currentSample;
        private int _prevSample;

        /// <summary>
        /// Get the most recent sampled values for all buttons, indexed by <see cref="Button"/>
        /// </summary>
        public bool[] CurrentButtonValues => this._buttonStates[this._currentSample];

        /// <summary>
        /// Get the previous sampled values for all buttons, indexed by <see cref="Button"/>
        /// </summary>
        public bool[] PreviousButtonValues => this._buttonStates[this._prevSample];

        /// <summary>
        /// Get the most recent sampled values for all axes, on a scale of (-1, 1),
        /// indexed by <see cref="Axis"/>
        /// </summary>
        public float[] CurrentAxisValues => this._axisStates[this._currentSample];

        /// <summary>
        /// Get the previous sampled values for all axes, on a scale of (-1, 1),
        /// indexed by <see cref="Axis"/>
        /// </summary>
        public float[] PreviousAxisValues => this._axisStates[this._prevSample];

        /// <summary>
        /// Get the most recent sampled value of the DPad
        /// </summary>
        public DPad CurrentDPadValue => this._dpadStates[this._currentSample];

        /// <summary>
        /// Get the previous sampled value of the DPad
        /// </summary>
        public DPad PreviousDPadValue => this._dpadStates[this._prevSample];

        /// <summary>
        /// Get the most recent sampled values from the gyro, if present,
        /// indexed by <see cref="GyroAxis"/>
        /// </summary>
        public float[] CurrentGyroValues => this._gyroStates[this._currentSample];

        /// <summary>
        /// Get the previous sampled values from the gyro, if present,
        /// indexed by <see cref="GyroAxis"/>
        /// </summary>
        public float[] PreviousGyroValues => this._gyroStates[this._prevSample];

        /// <summary>
        /// Get the most recent sampled values from the accelerometer, if present, 
        /// indexed by <see cref="AccelAxis"/>
        /// </summary>
        public float[] CurrentAccelValues => this._accelStates[this._currentSample];

        /// <summary>
        /// Get the previous sampled values from the accelerometer, if present, 
        /// indexed by <see cref="AccelAxis"/>
        /// </summary>
        public float[] PreviousAccelValues => this._accelStates[this._prevSample];

        internal unsafe Controller(int joystickIndex)
        {
            this.JoystickIndex = joystickIndex;
            this._controller = SDL_gamecontroller.SDL_GameControllerOpen(joystickIndex);
            this.HasRumble = SDL_gamecontroller.SDL_GameControllerHasRumble(this._controller) == Generated.Shared.SDL_bool.SDL_TRUE;
            this.HasGyro = SDL_gamecontroller.SDL_GameControllerHasSensor(this._controller, SDL_SensorType.SDL_SENSOR_GYRO) == Generated.Shared.SDL_bool.SDL_TRUE;
            if (this.HasGyro)
            {
                _ = SDL_gamecontroller.SDL_GameControllerSetSensorEnabled(this._controller, SDL_SensorType.SDL_SENSOR_GYRO, Generated.Shared.SDL_bool.SDL_TRUE);
            }

            this.HasAccel = SDL_gamecontroller.SDL_GameControllerHasSensor(this._controller, SDL_SensorType.SDL_SENSOR_ACCEL) == Generated.Shared.SDL_bool.SDL_TRUE;
            if (this.HasAccel)
            {
                _ = SDL_gamecontroller.SDL_GameControllerSetSensorEnabled(this._controller, SDL_SensorType.SDL_SENSOR_ACCEL, Generated.Shared.SDL_bool.SDL_TRUE);
            }

            this.Name = new string(SDL_gamecontroller.SDL_GameControllerName(this._controller));

            this._dpadStates = new DPad[2];
            this._buttonStates = new bool[2][];
            this._axisStates = new float[2][];
            this._gyroStates = new float[2][];
            this._accelStates = new float[2][];

            for (int i = 0; i < 2; i++)
            {
                this._buttonStates[i] = new bool[(int)SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_MAX];
                this._axisStates[i] = new float[(int)SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_MAX];
                this._gyroStates[i] = new float[3];
                this._accelStates[i] = new float[3];
            }
        }

        internal static bool IsController(int joystickId)
        {
            return SDL_gamecontroller.SDL_IsGameController(joystickId) == Generated.Shared.SDL_bool.SDL_TRUE;
        }

        /// <summary>
        /// Poll the controller, updating the current and previous state of each button, axis, and sensor
        /// </summary>
        public void Poll()
        {
            this._prevSample = this._currentSample;
            this._currentSample = (this._currentSample + 1) % 2;

            int sample = this._currentSample;
            _SDL_GameController* controller = this._controller;

            for (int i = 0; i < this._axisStates[sample].Length; i++)
            {
                short axisValue = SDL_gamecontroller.SDL_GameControllerGetAxis(controller, (SDL_GameControllerAxis)i);
                this._axisStates[sample][i] = Math.Clamp(axisValue / (float)short.MaxValue, -1, 1);
            }

            byte dpadValue = 0;
            byte dpadMul = 1;
            for (int i = 0; i < this._buttonStates[this._currentSample].Length; i++)
            {
                byte state = SDL_gamecontroller.SDL_GameControllerGetButton(controller, (SDL_GameControllerButton)i);
                this._buttonStates[sample][i] = state != 0;
                if (i >= (int)SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP && i <= (int)SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT)
                {
                    dpadValue += (byte)(state * dpadMul);
                    dpadMul *= 2;
                }
            }

            this._dpadStates[sample] = (DPad)dpadValue;

            if (this.HasGyro)
            {
                fixed (float* gyroStates = this._gyroStates[sample])
                {
                    _ = SDL_gamecontroller.SDL_GameControllerGetSensorData(controller, SDL_SensorType.SDL_SENSOR_GYRO, gyroStates, 3);
                }
            }

            if (this.HasAccel)
            {
                fixed (float* accelStates = this._accelStates[sample])
                {
                    _ = SDL_gamecontroller.SDL_GameControllerGetSensorData(controller, SDL_SensorType.SDL_SENSOR_ACCEL, accelStates, 3);
                }
            }
        }

        /// <summary>
        /// Make the device rumble for the specified duration; this will replace any current rumble effect.
        /// </summary>
        /// <param name="lowFrequency">Intensity value for low-frequency rumble motor</param>
        /// <param name="highFrequency">Intensity value for high-frquency rumble motor</param>
        /// <param name="durationMilliseconds">Duration of the rumble effect</param>
        public void Rumble(ushort lowFrequency, ushort highFrequency, uint durationMilliseconds)
        {
            _ = SDL_gamecontroller.SDL_GameControllerRumble(this._controller, lowFrequency, highFrequency, durationMilliseconds);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                SDL_gamecontroller.SDL_GameControllerClose(this._controller);
                this._controller = null;
                this._disposedValue = true;
            }
        }

        ~Controller()
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

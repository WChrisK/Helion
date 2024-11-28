namespace Helion.Client.Input.Controller
{
    using Helion.Audio.Sounds;
    using Helion.Window.Input;
    using SDLControllerWrapper;
    using System;
    using System.Linq;

    public class ControllerAdapter : IGameControlAdapter, IDisposable
    {
        public float AnalogDeadZone;
        private bool m_enabled;
        private bool m_rumbleEnabled;
        private readonly InputManager m_inputManager;
        private SDLControllerWrapper m_controllerWrapper;
        private bool m_disposedValue;

        private Controller? m_activeController;

        public ControllerAdapter(float analogDeadZone, bool enabled, bool rumbleEnabled, InputManager inputManager)
        {
            AnalogDeadZone = analogDeadZone;
            m_enabled = enabled;
            m_rumbleEnabled = rumbleEnabled;
            m_inputManager = inputManager;
            inputManager.AnalogAdapter = this;

            m_controllerWrapper = new SDLControllerWrapper(HandleConfigChange);
            m_activeController = m_controllerWrapper.Controllers.FirstOrDefault();
        }

        private void HandleConfigChange(object? sender, ConfigurationEvent configEvent)
        {
            if (configEvent.ChangeType == ConfigurationChange.Removed
                && configEvent.JoystickIndex == m_activeController?.JoystickIndex)
            {
                m_activeController = null;
            }

            if (configEvent.ChangeType == ConfigurationChange.Added
                && m_activeController == null)
            {
                m_activeController = m_controllerWrapper.Controllers.First();
            }
        }

        public void SetEnabled(bool enable)
        {
            if (enable)
            {
                m_controllerWrapper.DetectControllers();
                m_activeController = m_controllerWrapper.Controllers.FirstOrDefault();
            }
            else
            {
                // Ensure no buttons are "stuck" when we disable the controller.
                for (Key k = Key.LeftYPlus; k <= Key.DPadRight; k++)
                {
                    m_inputManager.SetKeyUp(k);
                }
            }
            m_enabled = enable;
        }

        public void SetRumbleEnabled(bool enable)
        {
            m_rumbleEnabled = enable;
        }

        public void Poll()
        {
            if (!m_enabled)
            {
                return;
            }

            // We must always poll, because this is also how we will detect if a controller is connected.
            m_controllerWrapper.Poll();
            if (m_activeController == null)
            {
                return;
            }

            // Check button states, send button updates
            for (int i = 0; i < m_activeController.CurrentButtonValues.Length; i++)
            {
                bool currentlyPressed = m_activeController.CurrentButtonValues[i];
                bool previouslyPressed = m_activeController.PreviousButtonValues[i];

                if (currentlyPressed && !previouslyPressed)
                {
                    m_inputManager.SetKeyDown(ControllerStatic.ButtonsToKeys[i]);
                }
                else if (previouslyPressed && !currentlyPressed)
                {
                    m_inputManager.SetKeyUp(ControllerStatic.ButtonsToKeys[i]);
                }
            }

            // Check axis states, send axis-as-button updates
            for (int i = 0; i < m_activeController.CurrentAxisValues.Length; i++)
            {
                float currentValue = m_activeController.CurrentAxisValues[i];
                float previousValue = m_activeController.PreviousAxisValues[i];

                bool isPositive = currentValue > AnalogDeadZone;
                bool isNegative = currentValue < -AnalogDeadZone;
                bool wasPositive = previousValue > AnalogDeadZone;
                bool wasNegative = previousValue < -AnalogDeadZone;

                (Key? axisNegative, Key axisPositive) = ControllerStatic.AxisToKeys[i];

                if (isPositive && !wasPositive)
                {
                    m_inputManager.SetKeyDown(axisPositive);
                }
                if (isNegative && !wasNegative)
                {
                    if (axisNegative != null)
                    {
                        m_inputManager.SetKeyDown(axisNegative.Value);
                    }
                }
                if (!isPositive && wasPositive)
                {
                    m_inputManager.SetKeyUp(axisPositive);
                }
                if (!isNegative && wasNegative)
                {
                    if (axisNegative != null)
                    {
                        m_inputManager.SetKeyUp(axisNegative.Value);
                    }
                }
            }
        }

        public bool TryGetAnalogValueForAxis(Key key, out float axisAnalogValue)
        {
            if (!m_enabled || m_activeController == null || !ControllerStatic.KeysToAxis.TryGetValue(key, out (int axisId, bool isPositive) axis))
            {
                axisAnalogValue = 0;
                return false;
            }

            axisAnalogValue = m_activeController.CurrentAxisValues[axis.axisId];
            axisAnalogValue = Math.Abs(axis.isPositive
                ? Math.Clamp(axisAnalogValue, 0, 1)
                : Math.Clamp(axisAnalogValue, -1, 0));
            axisAnalogValue = (axisAnalogValue - AnalogDeadZone) / (1 - AnalogDeadZone);

            return true;
        }

        public bool TryGetGyroAxis(GyroOrAccelAxis axis, out float value)
        {
            if (!m_enabled || m_activeController == null || !m_activeController.HasGyro)
            {
                value = 0;
                return false;
            }

            switch (axis)
            {
                case GyroOrAccelAxis.X:
                    value = m_activeController.CurrentAccelValues[0];
                    return true;
                case GyroOrAccelAxis.Y:
                    value = m_activeController.CurrentAccelValues[1];
                    return true;
                case GyroOrAccelAxis.Z:
                    value = m_activeController.CurrentAccelValues[2];
                    return true;
                case GyroOrAccelAxis.Pitch:
                    value = m_activeController.CurrentGyroValues[0];
                    return true;
                case GyroOrAccelAxis.Yaw:
                    value = m_activeController.CurrentGyroValues[1];
                    return true;
                case GyroOrAccelAxis.Roll:
                    value = m_activeController.CurrentGyroValues[2];
                    return true;
                default:
                    value = 0;
                    return false;
            }
        }

        public bool TryGetGyroAbsolute(Helion.Window.Input.GyroAxis axis, out double value)
        {
            if (!m_enabled || m_activeController == null || !m_activeController.HasGyro)
            {
                value = 0;
                return false;
            }

            value = m_activeController.CurrentGyroAbsolutePosition[(int)axis];
            return true;
        }

        public void ZeroGyroAbsolute()
        {
            m_activeController?.ZeroGyroAbsolute();
        }

        public void Rumble(ushort lowFrequency, ushort highFrequency, uint durationms)
        {
            if (m_enabled && m_rumbleEnabled && m_activeController?.HasRumble == true)
            {
                m_activeController.Rumble(lowFrequency, highFrequency, durationms);
            }
        }

        public void RumbleForSoundCreated(object? sender, SoundCreatedEventArgs evt)
        {
            if (!m_enabled || !m_rumbleEnabled || m_activeController?.HasRumble != true)
            {
                return;
            }

            if (evt.SoundParams.Context != null)
            {
                Audio.SoundContext ctx = evt.SoundParams.Context.Value;
                m_activeController.Rumble(ctx.LowFrequencyIntensity, ctx.HighFrequencyIntensity, ctx.DurationMilliseconds);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                m_controllerWrapper.Dispose();
                m_disposedValue = true;
            }
        }

        ~ControllerAdapter()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

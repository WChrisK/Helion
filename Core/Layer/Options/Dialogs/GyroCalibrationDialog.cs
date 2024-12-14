namespace Helion.Layer.Options.Dialogs
{
    using Helion.Geometry.Vectors;
    using Helion.Render.Common.Renderers;
    using Helion.Util.Configs.Components;
    using Helion.Window;
    using System;
    using System.Data.SqlTypes;
    using System.Timers;

    internal class GyroCalibrationDialog : DialogBase
    {
        private readonly ConfigController m_controllerConfig;
        private readonly IInputManager m_inputManager;
        private int m_calibrationStatus = 0; // 0 = inactive, 1 = in pre-calibration countdown, 2 = calibrating, 3 = done

        private readonly Timer m_timer;
        private int m_timerCountdown;
        private int m_calibrationCountdown;
        private string m_timerMessage = string.Empty;

        private const string NOGYRO = "Gyro not detected";
        private const string PRECALIBRATIONPROMPT = "Place controller on a flat surface";
        private const string CALIBRATIONPROMPT = "Calibrating; leave controller still";
        private const string CALIBRATIONFINISHED = "Calibration finished";

        public GyroCalibrationDialog(ConfigWindow config, ConfigController controllerConfig, IInputManager inputMgr)
            : base(config, "OK", "Cancel")
        {
            m_controllerConfig = controllerConfig;
            m_inputManager = inputMgr;
            m_timer = new Timer(1000);
            m_timer.AutoReset = true;
            m_timer.Elapsed += TimerElapsed;
        }

        protected override void RenderDialogContents(IRenderableSurfaceContext ctx, IHudRenderContext hud, bool sizeChanged)
        {
            hud.AddOffset((m_dialogOffset.X + m_padding, 0));
            if (m_inputManager.AnalogAdapter?.HasGyro != true)
            {
                m_timer.Stop();
                m_calibrationStatus = 0;
                m_timerMessage = string.Empty;

                RenderDialogText(hud, NOGYRO);
            }
            else
            {
                switch (m_calibrationStatus)
                {
                    case 0:
                        m_calibrationStatus = 1;
                        m_timerCountdown = 5;
                        m_calibrationCountdown = 10;

                        m_timer.Start();
                        break;
                    case 1:
                        RenderDialogText(hud, PRECALIBRATIONPROMPT);
                        RenderDialogText(hud, m_timerMessage);
                        break;
                    case 2:
                        RenderDialogText(hud, CALIBRATIONPROMPT);
                        RenderDialogText(hud, m_timerMessage);
                        break;
                    default:
                        RenderDialogText(hud, CALIBRATIONFINISHED);
                        break;

                }
            }
        }

        private void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (m_calibrationStatus < 2)
            {
                m_timerMessage = $"Waiting: {m_timerCountdown}";
                m_timerCountdown--;
                if (m_timerCountdown == 0)
                {
                    m_timerMessage = string.Empty;
                    m_calibrationStatus = 2;
                    m_inputManager.AnalogAdapter?.CalibrateGyro(4000, CalibrationFinished);
                }
            }
            else if (m_calibrationStatus == 2)
            {
                m_timerMessage = $"Calibrating: {Math.Max(0, m_calibrationCountdown)}";
                m_calibrationCountdown--;
            }
        }

        private void CalibrationFinished(Vec3F noise, Vec3F drift)
        {
            m_timer.Stop();
            m_controllerConfig.GyroNoise.Set(noise);
            m_controllerConfig.GyroDrift.Set(drift);
            m_calibrationStatus = 3;
        }
    }
}

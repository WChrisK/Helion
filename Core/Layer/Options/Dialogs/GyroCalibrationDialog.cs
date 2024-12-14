namespace Helion.Layer.Options.Dialogs
{
    using Helion.Render.Common.Renderers;
    using Helion.Util.Configs.Components;
    using Helion.Window;

    internal class GyroCalibrationDialog : DialogBase
    {
        private readonly ConfigController m_controllerConfig;
        private readonly IInputManager m_inputManager;

        private const string NOGYRO = "Gyro not detected";
        private const string CALIBRATIONPROMPT = "Place controller on a flat surface";

        public GyroCalibrationDialog(ConfigWindow config, ConfigController controllerConfig, IInputManager inputMgr)
            : base(config, "OK", "Cancel")
        {
            m_controllerConfig = controllerConfig;
            m_inputManager = inputMgr;
        }

        protected override void RenderDialogContents(IRenderableSurfaceContext ctx, IHudRenderContext hud, bool sizeChanged)
        {
            hud.AddOffset((m_dialogOffset.X + m_padding, 0));
            if (m_inputManager.AnalogAdapter?.HasGyro != true)
            {
                RenderDialogText(hud, NOGYRO);
            }
            else
            {
                RenderDialogText(hud, CALIBRATIONPROMPT);
            }
        }
    }
}

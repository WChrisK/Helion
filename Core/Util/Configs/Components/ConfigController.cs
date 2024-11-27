namespace Helion.Util.Configs.Components;

using Helion.Util.Configs.Impl;
using Helion.Util.Configs.Options;
using Helion.Util.Configs.Values;
using static Helion.Util.Configs.Values.ConfigFilters;

public enum GyroTurnAxis
{
    Yaw = 1,
    Roll = 2
}

public class ConfigController : ConfigElement<ConfigController>
{
    // Controller

    [ConfigInfo("Enable game controller support.")]
    [OptionMenu(OptionSectionType.Controller, "Enable Game Controller", spacer: true)]
    public readonly ConfigValue<bool> EnableGameController = new(true);

    [ConfigInfo("Dead zone for analog inputs.")]
    [OptionMenu(OptionSectionType.Controller, "Dead Zone", sliderMin: 0.1, sliderMax: 0.9, sliderStep: .05)]
    public readonly ConfigValue<double> GameControllerDeadZone = new(0.2, Clamp(0.1, 0.9));

    [ConfigInfo("Turn speed scaling factor for analog inputs.")]
    [OptionMenu(OptionSectionType.Controller, "Turn Sensitivity", sliderMin: 0.1, sliderMax: 3.0, sliderStep: .05)]
    public readonly ConfigValue<double> GameControllerTurnScale = new(1.0, Clamp(0.1, 3.0));

    [ConfigInfo("Pitch speed scaling factor for analog inputs.")]
    [OptionMenu(OptionSectionType.Controller, "Pitch Sensitivity", sliderMin: 0.1, sliderMax: 3.0, sliderStep: .05)]
    public readonly ConfigValue<double> GameControllerPitchScale = new(0.5, Clamp(0.1, 3.0));

    [ConfigInfo("Run input scaling factor for analog inputs.")]
    [OptionMenu(OptionSectionType.Controller, "Run Sensitivity", sliderMin: 0.1, sliderMax: 3.0, sliderStep: .05)]
    public readonly ConfigValue<double> GameControllerRunScale = new(1.0, Clamp(0.1, 3.0));

    [ConfigInfo("Strafe input scaling factor for analog inputs.")]
    [OptionMenu(OptionSectionType.Controller, "Strafe Sensitivity", sliderMin: 0.1, sliderMax: 3.0, sliderStep: .05)]
    public readonly ConfigValue<double> GameControllerStrafeScale = new(1.0, Clamp(0.1, 3.0));

    // Gyro aiming

    [ConfigInfo("Gyro axis to use for turning left and right.")]
    [OptionMenu(OptionSectionType.Controller, "Gyro Aim Turn Axis")]
    public readonly ConfigValue<GyroTurnAxis> GyroAimTurnAxis = new(GyroTurnAxis.Yaw);

    [ConfigInfo("Vertical aiming sensitivity for gyro input.")]
    [OptionMenu(OptionSectionType.Controller, "Gyro Aim Vertical Sensitivity", sliderMin: 0, sliderMax: 10, sliderStep: .1)]
    public readonly ConfigValue<double> GyroAimVerticalSensitivity = new(3.0, Clamp(0, 10.0));

    [ConfigInfo("Horizontal aiming sensitivity for gyro input.")]
    [OptionMenu(OptionSectionType.Controller, "Gyro Aim Turn Sensitivity", sliderMin: 0, sliderMax: 10, sliderStep: .1)]
    public readonly ConfigValue<double> GyroAimHorizontalSensitivity = new(3.0, Clamp(0, 10.0));
}


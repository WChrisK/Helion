namespace Helion.Util.Configs.Impl
{
    using Helion.Window.Input;
    using System.Collections.Generic;
    using System.ComponentModel;

    public enum ControllerPresetType
    {
        None,
        Custom,
        [Description("XBOX One")]
        XBoxOne,
        [Description("DualShock 4")]
        PS4
    }

    public partial class ConfigKeyMapping
    {
        // These are intended as default mappings for common controller types.

        public static readonly Dictionary<ControllerPresetType, (Key key, string command)[]> ControllerPresetMappings =
            new()
            {
                { ControllerPresetType.None, [] },
                { ControllerPresetType.XBoxOne,
                    [
                        (Key.LeftYMinus,        Constants.Input.Forward),
                        (Key.LeftYPlus,         Constants.Input.Backward),
                        (Key.LeftXMinus,        Constants.Input.Left),
                        (Key.LeftXPlus,         Constants.Input.Right),
                        (Key.RightXMinus,       Constants.Input.TurnLeft),
                        (Key.RightXPlus,        Constants.Input.TurnRight),
                        (Key.ButtonB,           Constants.Input.Use),
                        (Key.ButtonX,           Constants.Input.Use),
                        (Key.ButtonA,           Constants.Input.Attack),
                        (Key.RightTriggerPlus,  Constants.Input.Attack),
                        (Key.DPadUp,            Constants.Input.NextWeapon),
                        (Key.DPadDown,          Constants.Input.PreviousWeapon),
                        (Key.ButtonStart,       Constants.Input.Menu),
                    ]},
                { ControllerPresetType.PS4,
                    [
                        (Key.LeftYMinus,        Constants.Input.Forward),
                        (Key.LeftYPlus,         Constants.Input.Backward),
                        (Key.LeftXMinus,        Constants.Input.Left),
                        (Key.LeftXPlus,         Constants.Input.Right),
                        (Key.RightXMinus,       Constants.Input.TurnLeft),
                        (Key.RightXPlus,        Constants.Input.TurnRight),
                        (Key.ButtonB,           Constants.Input.Use),
                        (Key.ButtonX,           Constants.Input.Use),
                        (Key.ButtonA,           Constants.Input.Attack),
                        (Key.RightTriggerPlus,  Constants.Input.Attack),
                        (Key.DPadUp,            Constants.Input.NextWeapon),
                        (Key.DPadDown,          Constants.Input.PreviousWeapon),
                        (Key.ButtonStart,       Constants.Input.Menu),
                    ]}
            };
    }
}

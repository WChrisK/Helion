namespace Helion.Util.Configs.Impl
{
    using Helion.Window.Input;
    using System.Collections.Generic;
    using System.ComponentModel;

    public enum ControllerPresetType
    {
        None,
        Custom,
        [Description("XBOX One (Windows)")]
        XBoxOneWindows,
        [Description("PS4 (Windows)")]
        PS4Windows
    }

    public partial class ConfigKeyMapping
    {
        // These are intended as default mappings for custom controller types.

        public static readonly Dictionary<ControllerPresetType, (Key key, string command)[]> ControllerPresetMappings = new()
        {
            { ControllerPresetType.None, [] },
            { ControllerPresetType.XBoxOneWindows,
                [
                    (Key.Axis2Minus,    Constants.Input.Forward),
                    (Key.Axis2Plus,     Constants.Input.Backward),
                    (Key.Axis1Minus,    Constants.Input.Left),
                    (Key.Axis1Plus,     Constants.Input.Right),
                    (Key.Axis3Minus,    Constants.Input.TurnLeft),
                    (Key.Axis3Plus,     Constants.Input.TurnRight),
                    (Key.Button3,       Constants.Input.Use),
                    (Key.Button1,       Constants.Input.Attack),
                    (Key.Axis6Plus,     Constants.Input.Attack),
                    (Key.DPad1Up,       Constants.Input.NextWeapon),
                    (Key.DPad1Down,     Constants.Input.PreviousWeapon),
                    (Key.Button8,       Constants.Input.Menu),
                ]},
            { ControllerPresetType.PS4Windows,
                [
                    (Key.Axis2Minus,    Constants.Input.Forward),
                    (Key.Axis2Plus,     Constants.Input.Backward),
                    (Key.Axis1Minus,    Constants.Input.Left),
                    (Key.Axis1Plus,     Constants.Input.Right),
                    (Key.Axis3Minus,    Constants.Input.TurnLeft),
                    (Key.Axis3Plus,     Constants.Input.TurnRight),
                    (Key.Button2,       Constants.Input.Use),
                    (Key.Button1,       Constants.Input.Attack),
                    (Key.Axis5Plus,     Constants.Input.Attack),
                    (Key.DPad1Up,       Constants.Input.NextWeapon),
                    (Key.DPad1Down,     Constants.Input.PreviousWeapon),
                    (Key.Button13,      Constants.Input.Menu),
                    (Key.Button14,      Constants.Input.Menu),
                ]}
        };
    }
}

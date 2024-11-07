namespace Helion.Util.Configs.Impl
{
    using Helion.Window.Input;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

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
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            // Windows controller mappings           
            new()
            {
                { ControllerPresetType.None, [] },
                { ControllerPresetType.XBoxOne,
                    [
                        (Key.Axis2Minus,    Constants.Input.Forward),
                        (Key.Axis2Plus,     Constants.Input.Backward),
                        (Key.Axis1Minus,    Constants.Input.Left),
                        (Key.Axis1Plus,     Constants.Input.Right),
                        (Key.Axis3Minus,    Constants.Input.TurnLeft),
                        (Key.Axis3Plus,     Constants.Input.TurnRight),
                        (Key.Button2,       Constants.Input.Use),
                        (Key.Button3,       Constants.Input.Use),
                        (Key.Button1,       Constants.Input.Attack),
                        (Key.Axis6Plus,     Constants.Input.Attack),
                        (Key.DPad1Up,       Constants.Input.NextWeapon),
                        (Key.DPad1Down,     Constants.Input.PreviousWeapon),
                        (Key.Button8,       Constants.Input.Menu),
                    ]},
                { ControllerPresetType.PS4,
                    [
                        (Key.Axis2Minus,    Constants.Input.Forward),
                        (Key.Axis2Plus,     Constants.Input.Backward),
                        (Key.Axis1Minus,    Constants.Input.Left),
                        (Key.Axis1Plus,     Constants.Input.Right),
                        (Key.Axis3Minus,    Constants.Input.TurnLeft),
                        (Key.Axis3Plus,     Constants.Input.TurnRight),
                        (Key.Button1,       Constants.Input.Use),
                        (Key.Button3,       Constants.Input.Use),
                        (Key.Button2,       Constants.Input.Attack),
                        (Key.Axis5Plus,     Constants.Input.Attack),
                        (Key.DPad1Up,       Constants.Input.NextWeapon),
                        (Key.DPad1Down,     Constants.Input.PreviousWeapon),
                        (Key.Button13,      Constants.Input.Menu),
                        (Key.Button14,      Constants.Input.Menu),
                    ]}
            } :
            // Linux controller mappings
            new()
            {
                { ControllerPresetType.None, [] },
                { ControllerPresetType.XBoxOne,
                    [
                        (Key.Axis2Minus,    Constants.Input.Forward),
                        (Key.Axis2Plus,     Constants.Input.Backward),
                        (Key.Axis1Minus,    Constants.Input.Left),
                        (Key.Axis1Plus,     Constants.Input.Right),
                        (Key.Axis4Minus,    Constants.Input.TurnLeft),
                        (Key.Axis4Plus,     Constants.Input.TurnRight),
                        (Key.Button2,       Constants.Input.Use),
                        (Key.Button3,       Constants.Input.Use),
                        (Key.Button1,       Constants.Input.Attack),
                        (Key.Axis6Plus,     Constants.Input.Attack),
                        (Key.DPad1Up,       Constants.Input.NextWeapon),
                        (Key.DPad1Down,     Constants.Input.PreviousWeapon),
                        (Key.Button8,       Constants.Input.Menu),
                    ]},
                { ControllerPresetType.PS4,
                    [
                        (Key.Axis2Minus,    Constants.Input.Forward),
                        (Key.Axis2Plus,     Constants.Input.Backward),
                        (Key.Axis1Minus,    Constants.Input.Left),
                        (Key.Axis1Plus,     Constants.Input.Right),
                        (Key.Axis4Minus,    Constants.Input.TurnLeft),
                        (Key.Axis4Plus,     Constants.Input.TurnRight),
                        (Key.Button2,       Constants.Input.Use),
                        (Key.Button4,       Constants.Input.Use),
                        (Key.Button1,       Constants.Input.Attack),
                        (Key.Axis6Plus,     Constants.Input.Attack),
                        (Key.DPad1Up,       Constants.Input.NextWeapon),
                        (Key.DPad1Down,     Constants.Input.PreviousWeapon),
                        (Key.Button11,      Constants.Input.Menu),
                    ]}
            };
    }
}

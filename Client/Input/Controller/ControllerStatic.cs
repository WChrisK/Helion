namespace Helion.Client.Input.Controller
{
    using Helion.Window.Input;
    using System.Collections.Generic;

    public static class ControllerStatic
    {

        /// <summary>
        /// Maps a controller button index to a keypress
        /// </summary>
        public static readonly Key[] ButtonsToKeys =
        [
            Key.ButtonA,
            Key.ButtonB,
            Key.ButtonX,
            Key.ButtonY,
            Key.ButtonBack,
            Key.ButtonGuide,
            Key.ButtonStart,
            Key.ButtonLeftStick,
            Key.ButtonRightStick,
            Key.ButtonLeftShoulder,
            Key.ButtonRightShoulder,
            Key.DPadUp,
            Key.DPadDown,
            Key.DPadLeft,
            Key.DPadRight,
            Key.ButtonMisc1,
            Key.ButtonPaddle1,
            Key.ButtonPaddle2,
            Key.ButtonPaddle3,
            Key.ButtonPaddle4,
            Key.ButtonTouchpad,
        ];

        /// <summary>
        /// Maps a controller axis to "negative axis" and "positive axis" keypresses
        /// </summary>
        public static readonly (Key? axisNegative, Key axisPositive)[] AxisToKeys =
        {
            (Key.LeftXMinus, Key.LeftXPlus),
            (Key.LeftYMinus, Key.LeftYPlus),
            (Key.RightXMinus, Key.RightXPlus),
            (Key.RightYMinus, Key.RightYPlus),
            (null, Key.LeftTriggerPlus),
            (null, Key.RightTriggerPlus)
        };

        /// <summary>
        /// Maps a keypress back to an axis index and a "direction" (positive/negative)
        /// </summary>
        public static readonly Dictionary<Key, (int axisId, bool isPositive)> KeysToAxis = new()
        {
            { Key.LeftXPlus, (0, true) },
            { Key.LeftXMinus, (0, false) },
            { Key.LeftYPlus, (1, true) },
            { Key.LeftYMinus, (1, false) },
            { Key.RightXPlus, (2, true) },
            { Key.RightXMinus, (2, false) },
            { Key.RightYPlus, (3, true) },
            { Key.RightYMinus, (3, false) },
            { Key.LeftTriggerPlus, (4, true) },
            { Key.RightTriggerPlus, (5, true) }
        };
    }
}

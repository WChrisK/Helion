namespace SDLControllerWrapper
{
    using System;

    [Flags]
    public enum DPad : byte
    {
        Centered = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }

    public enum Button
    {
        A,
        B,
        X,
        Y,
        Back,
        Guide,
        Start,
        LeftStick,
        RightStick,
        LeftShoulder,
        RightShoulder,
        DPad_Up,
        DPad_Down,
        DPad_Left,
        Dpad_Right,
        Misc1,
        Paddle1,
        Paddle2,
        Paddle3,
        Paddle4,
        Touchpad,
    }

    public enum Axis
    {
        LeftX,
        LeftY,
        RightX,
        RightY,
        LeftTrigger,
        RightTrigger,
    }

    public enum AccelAxis
    {
        X,
        Y,
        Z
    }

    public enum GyroAxis
    {
        Pitch,
        Yaw,
        Roll
    }
}

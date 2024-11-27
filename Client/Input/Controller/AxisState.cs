namespace Helion.Client.Input.Controller
{
    public struct AxisState(float position, float positionCorrected, float delta, bool pressedPositive, bool pressedNegative)
    {
        public float Position = position;
        public float PositionCorrected = positionCorrected;
        public float Delta = delta;
        public bool PressedPositive = pressedPositive;
        public bool PressedNegative = pressedNegative;
    }
}

namespace SDLControllerWrapper
{
    public enum ConfigurationChange
    {
        Added,
        Removed,
        Remapped
    }

    public class ConfigurationEvent(ConfigurationChange changeType, int joystickIndex)
    {
        public readonly ConfigurationChange ChangeType = changeType;
        public readonly int JoystickIndex = joystickIndex;
    }
}

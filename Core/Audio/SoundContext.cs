namespace Helion.Audio;

public struct SoundContext(SoundEventType eventType, ushort lowFrequencyIntensity, ushort highFrequencyIntensity, uint durationMilliseconds)
{
    public SoundEventType EventType = eventType;
    public ushort LowFrequencyIntensity = lowFrequencyIntensity;
    public ushort HighFrequencyIntensity = highFrequencyIntensity;
    public uint DurationMilliseconds = durationMilliseconds;
}

public enum SoundEventType
{
    Default,
    WeaponFired,
    DamageReceived,
    MeleeWeaponHit
}

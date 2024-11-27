namespace Helion.Audio.Sounds
{
    using Helion.Resources.Definitions.SoundInfo;
    using Helion.World.Sound;

    public struct SoundCreatedEventArgs
    {
        public ISoundSource? SoundSource;
        public SoundParams SoundParams;
        public SoundInfo? SoundInfo;
    }
}

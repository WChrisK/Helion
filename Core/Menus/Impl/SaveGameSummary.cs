namespace Helion.Menus.Impl
{
    using Helion.Graphics;
    using Helion.Render.Common.Renderers;
    using Helion.Render.Common.Textures;
    using Helion.Util.Extensions;
    using Helion.World.Save;
    using System;

    public class SaveGameSummary : IDisposable
    {
        public readonly IRenderableTextureHandle? SaveGameImage;
        public readonly string MapName;
        public readonly DateTime? Date;
        public readonly string[] Stats;
        private readonly Action? m_saveGameTextureRemove;
        private bool disposedValue;

        public SaveGameSummary(SaveGame saveGame, IHudRenderContext hud)
        {
            MapName = saveGame.Model?.MapName ?? string.Empty;
            Date = saveGame.Model?.Date;
            Image? tempImage = saveGame.GetSaveGameImage();

            Stats = saveGame.Model?.SaveGameStats == null
                ? Array.Empty<string>()
                : [
                    $"Kills: {saveGame.Model.SaveGameStats.KillCount} / {saveGame.Model.SaveGameStats.TotalMonsters}",
                    $"Secrets: {saveGame.Model.SaveGameStats.SecretCount} / {saveGame.Model.SaveGameStats.TotalSecrets}",
                    $"Elapsed: {TimeSpan.FromSeconds(saveGame.Model.SaveGameStats.LevelTime / 35)}"
                ];

            if (tempImage != null)
            {
                SaveGameImage = hud.CreateImage(tempImage, saveGame.FileName, Resources.ResourceNamespace.Textures, out m_saveGameTextureRemove);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_saveGameTextureRemove?.Invoke();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

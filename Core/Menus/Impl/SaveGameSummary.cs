namespace Helion.Menus.Impl
{
    using Helion.Graphics;
    using Helion.Render.Common.Renderers;
    using Helion.Render.Common.Textures;
    using Helion.Util.Extensions;
    using Helion.World.Save;
    using System;

    public class SaveGameSummary
    {
        private const string TEXTURENAME = "SAVEGAMETHUMBNAIL";

        public readonly IRenderableTextureHandle? SaveGameImage;
        public readonly string MapName;
        public readonly DateTime? Date;
        public readonly string[] Stats;
        private Image? m_saveGameImage;

        public SaveGameSummary(SaveGame saveGame)
        {
            MapName = saveGame.Model?.MapName ?? string.Empty;
            Date = saveGame.Model?.Date;
            m_saveGameImage = saveGame.GetSaveGameImage();

            Stats = saveGame.Model?.SaveGameStats == null
                ? Array.Empty<string>()
                : [
                    $"Kills: {saveGame.Model.SaveGameStats.KillCount} / {saveGame.Model.SaveGameStats.TotalMonsters}",
                    $"Secrets: {saveGame.Model.SaveGameStats.SecretCount} / {saveGame.Model.SaveGameStats.TotalSecrets}",
                    $"Elapsed: {TimeSpan.FromSeconds(saveGame.Model.SaveGameStats.LevelTime / 35)}"
                ];
        }

        public IRenderableTextureHandle? UpdateSaveGameTexture(IHudRenderContext hud)
        {
            if (m_saveGameImage != null)
            {
                return hud.CreateOrReplaceImage(m_saveGameImage, TEXTURENAME, Resources.ResourceNamespace.Textures, false);
            }

            return null;
        }
    }
}

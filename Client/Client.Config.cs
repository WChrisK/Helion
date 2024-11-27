using Helion.Client.Music;
using Helion.Geometry;
using Helion.Util.Configs.Components;
using Helion.World.Entities.Players;
using OpenTK.Windowing.Common;
using System;

namespace Helion.Client;

public partial class Client
{
    private void RegisterConfigChanges()
    {
        m_config.Audio.MusicVolume.OnChanged += MusicVolume_OnChanged;
        m_config.Audio.SoundVolume.OnChanged += SoundVolume_OnChanged;

        m_config.Mouse.Look.OnChanged += Look_OnChanged;

        m_config.Window.State.OnChanged += WindowState_OnChanged;
        m_config.Window.Dimension.OnChanged += WindowDimension_OnChanged;
        m_config.Window.Border.OnChanged += WindowBorder_OnChanged;
        m_config.Window.Display.OnChanged += WindowDisplay_OnChanged;
        m_config.Window.Virtual.Enable.OnChanged += WindowVirtualEnable_OnChanged;
        m_config.Window.Virtual.Dimension.OnChanged += WindowVirtualDimension_OnChanged;

        m_config.Hud.AutoScale.OnChanged += AutoScale_OnChanged;

        m_config.Compatibility.SessionCompatLevel.OnChanged += SessionCompatLevel_OnChanged;

        CalculateHudScale();
    }

    private void CalculateHudScale()
    {
        if (!m_config.Hud.AutoScale)
            return;

        int ratio = Math.Clamp((int)Math.Ceiling(m_window.Size.Y / 799.0), 1, 10);
        m_config.Hud.Scale.Set(ratio);
    }

    private void AutoScale_OnChanged(object? sender, bool set)
    {
        if (set)
            CalculateHudScale();
    }

    private void WindowDisplay_OnChanged(object? sender, int display) =>
        m_window.SetDisplay(display);

    private void WindowVirtualEnable_OnChanged(object? sender, bool set) =>
        CalculateHudScale();

    private void WindowVirtualDimension_OnChanged(object? sender, Dimension dim) =>
        CalculateHudScale();

    private void WindowBorder_OnChanged(object? sender, WindowBorder border)
    {
        m_window.SetBorder(border);
    }

    private void WindowDimension_OnChanged(object? sender, Dimension dimension)
    {
        m_window.SetDimension(dimension);
        CalculateHudScale();
    }

    private void WindowState_OnChanged(object? sender, RenderWindowState state)
    {
        m_window.SetWindowState(state);
        CalculateHudScale();
    }

    private void Look_OnChanged(object? sender, bool set)
    {
        if (m_layerManager.WorldLayer == null || set)
            return;

        m_layerManager.WorldLayer.AddCommand(TickCommands.CenterView);
    }

    private void UnregisterConfigChanges()
    {
        m_config.Audio.MusicVolume.OnChanged -= MusicVolume_OnChanged;
        m_config.Audio.SoundVolume.OnChanged -= SoundVolume_OnChanged;
        m_config.Mouse.Look.OnChanged -= Look_OnChanged;
    }

    private void SoundVolume_OnChanged(object? sender, double volume) => UpdateVolume();

    private void MusicVolume_OnChanged(object? sender, double volume) => UpdateVolume();

    private void UpdateVolume()
    {
        m_audioSystem.SetVolume(m_config.Audio.SoundVolume);
        m_audioSystem.Music.SetVolume((float)m_config.Audio.MusicVolumeNormalized);
    }

    private void SessionCompatLevel_OnChanged(object? sender, Resources.Definitions.CompLevel e)
    {
        m_archiveCollection.Definitions.CompLevelDefinition.CompLevel = e;
        m_archiveCollection.Definitions.CompLevelDefinition.Apply(m_config, true);
    }
}

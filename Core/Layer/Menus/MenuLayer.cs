using System;
using System.Collections.Generic;
using System.Diagnostics;
using Helion.Audio.Sounds;
using Helion.Layer.Options;
using Helion.Menus;
using Helion.Menus.Impl;
using Helion.Render.Common.Renderers;
using Helion.Resources.Archives.Collection;
using Helion.Util;
using Helion.Util.Configs;
using Helion.Util.Consoles;
using Helion.Util.Timing;
using Helion.World.Save;

namespace Helion.Layer.Menus;

public partial class MenuLayer : IGameLayer, IAnimationLayer
{
    public InterpolationAnimation<IAnimationLayer> Animation { get; }
    internal readonly GameLayerManager Manager;
    private readonly IConfig m_config;
    private readonly HelionConsole m_console;
    private readonly ArchiveCollection m_archiveCollection;
    private readonly SoundManager m_soundManager;
    private readonly SaveGameManager m_saveGameManager;
    private readonly Stack<Menu> m_menus = new();
    private readonly Stopwatch m_stopwatch = new();
    private readonly OptionsLayer m_optionsLayer;
    private readonly IScreenshotGenerator m_screenshotGenerator;
    private readonly Action<IHudRenderContext> m_renderVirtualHudAction;
    private bool m_disposed;

    public MenuLayer(GameLayerManager manager, IConfig config, HelionConsole console,
        ArchiveCollection archiveCollection, SoundManager soundManager, SaveGameManager saveGameManager, OptionsLayer optionsLayer, IScreenshotGenerator screenshotGenerator)
    {
        Manager = manager;
        m_config = config;
        m_console = console;
        m_archiveCollection = archiveCollection;
        m_soundManager = soundManager;
        m_saveGameManager = saveGameManager;
        m_optionsLayer = optionsLayer;
        m_screenshotGenerator = screenshotGenerator;
        m_renderVirtualHudAction = new(RenderVirtualHud);
        m_stopwatch.Start();

        Animation = new(TimeSpan.FromMilliseconds(200), this);

        MainMenu mainMenu = new(this, config, console, soundManager, archiveCollection, saveGameManager, optionsLayer, m_screenshotGenerator);
        m_menus.Push(mainMenu);
    }

    public bool ShouldRemove()
    {
        return Animation.State == InterpolationAnimationState.OutComplete;
    }

    public void AddSaveOrLoadMenuIfMissing(bool isSave, bool clearOnExit)
    {
        foreach (Menu menu in m_menus)
            if (menu.GetType() == typeof(SaveMenu))
                return;

        bool hasWorld = Manager.WorldLayer != null;
        SaveMenu saveMenu = new(this, m_config, m_console, m_soundManager, m_archiveCollection,
            m_saveGameManager, m_screenshotGenerator, hasWorld, isSave, clearOnExit);

        m_menus.Push(saveMenu);
    }

    public void ShowOptionsMenu()
    {
        m_optionsLayer.ClearOnExit = true;
        m_optionsLayer.Animation.AnimateIn();
        Manager.Add(m_optionsLayer);
    }

    public void ShowMessage(MessageMenu message)
    {
        m_menus.Push(message);
    }

    public void SetToMainMenu()
    {
        m_menus.Clear();
        m_menus.Push(new MainMenu(this, m_config, m_console, m_soundManager, m_archiveCollection, m_saveGameManager, m_optionsLayer, m_screenshotGenerator));
    }

    public void Close()
    {
        Animation.AnimateOut();
    }

    public void RunLogic(TickerInfo tickerInfo)
    {
        // No logic.
    }

    public void Dispose()
    {
        if (m_disposed)
            return;

        // This comes first because when we're removing ourselves from the
        // parent, we run into an infinite loop. This short-circuits it.
        m_disposed = true;

        m_menus.Clear();

        Manager.Remove(this);
    }
}

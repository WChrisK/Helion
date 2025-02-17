using Helion.Geometry;
using Helion.Util.Configs.Impl;
using Helion.Util.Configs.Options;
using Helion.Util.Configs.Values;
using OpenTK.Windowing.Common;
using System.ComponentModel;
using static Helion.Util.Configs.Values.ConfigFilters;

namespace Helion.Util.Configs.Components;

public enum RenderWindowState
{
    [Description("Window")]
    Normal,
    [Description("Full Screen (exclusive)")]
    Fullscreen,
    [Description("Full Screen (borderless window)")]
    BorderlessFullscreenWindow
}

public enum BlitFilter
{
    Auto,
    Nearest,
    Linear
}

public class ConfigWindowVirtual: ConfigElement<ConfigWindowVirtual>
{
    [ConfigInfo("Virtual screen size.")]
    [OptionMenu(OptionSectionType.Video, "Virtual Size", spacer: true)]
    public readonly ConfigValue<Dimension> Dimension = new((800, 600), (_, dim) => dim.Width >= 320 && dim.Height >= 200);

    [ConfigInfo("Use virtual screen size.")]
    [OptionMenu(OptionSectionType.Video, "Use Virtual Size")]
    public readonly ConfigValue<bool> Enable = new(false);

    [ConfigInfo("Stretch the image if widescreen, or render black bars on the sides.")]
    [OptionMenu(OptionSectionType.Video, "Stretch Virtual Size")]
    public readonly ConfigValue<bool> Stretch = new(false);

    [ConfigInfo("Filter algorithm for virtual screens size.")]
    [OptionMenu(OptionSectionType.Video, "Virtual Filter")]
    public readonly ConfigValue<BlitFilter> Filter = new(BlitFilter.Auto);
}

public class ConfigWindow: ConfigElement<ConfigWindow>
{
    [ConfigInfo("Display fullscreen or windowed.")]
    [OptionMenu(OptionSectionType.Video, "Fullscreen/Window", allowReset: false)]
    public readonly ConfigValue<RenderWindowState> State = new(RenderWindowState.Fullscreen, OnlyValidEnums<RenderWindowState>());

    [ConfigInfo("Window border.")]
    [OptionMenu(OptionSectionType.Video, "Border")]
    public readonly ConfigValue<WindowBorder> Border = new(WindowBorder.Resizable, OnlyValidEnums<WindowBorder>());

    [ConfigInfo("Window width and height.")]
    [OptionMenu(OptionSectionType.Video, "Window Size")]
    public readonly ConfigValue<Dimension> Dimension = new((1024, 768), (_, dim) => dim.Width >= 320 && dim.Height >= 200);

    [ConfigInfo("Amount to scale menu text.")]
    [OptionMenu(OptionSectionType.Video, "Menu Scale", allowReset: false, sliderMin: 0, sliderMax: 10, sliderStep: 1)]
    public readonly ConfigValue<double> MenuScale = new(2.0, Greater(0.0));

    public readonly ConfigWindowVirtual Virtual = new();

    [ConfigInfo("Display number for the window. Use command ListDisplays for display numbers.")]
    [OptionMenu(OptionSectionType.Video, "Display Number", spacer: true, allowReset: false, sliderMin: 0, sliderMax: 10, sliderStep: 1)]
    public readonly ConfigValue<int> Display = new(0, GreaterOrEqual(0));

    [ConfigInfo("Color rendering mode: Palette uses Doom's colormaps and disables texture filtering, producing output that resembles software rendering. True Color interpolates color values. Application restart required.", restartRequired: true)]
    [OptionMenu(OptionSectionType.Video, "Color Mode", allowReset: false)]
    public readonly ConfigValue<RenderColorMode> ColorMode = new(RenderColorMode.TrueColor);
}

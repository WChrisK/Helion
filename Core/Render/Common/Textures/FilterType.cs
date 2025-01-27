using System.ComponentModel;

namespace Helion.Render.Common.Textures;

public enum FilterType
{
    Nearest,
    Bilinear,
    Trilinear,
    [Description("Nearest to bilinear")]
    NeareastBilinear,
    [Description("Nearest to trilinear")]
    NearestTrilinear
}

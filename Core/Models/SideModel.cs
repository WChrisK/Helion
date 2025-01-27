namespace Helion.Models;

public struct SideModel
{
    public int DataChanges { get; set; }
    // Integer texture handles are deprecated here. Keeping for backwards compatibiity.
    public int? UpperTexture { get; set; }
    public int? MiddleTexture { get; set; }
    public int? LowerTexture { get; set; }
    public string? UpperTex { get; set; }
    public string? MiddelTex { get; set; }
    public string? LowerTex { get; set; }
}

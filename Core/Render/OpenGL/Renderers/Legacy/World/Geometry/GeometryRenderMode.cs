namespace Helion.Render.OpenGL.Renderers.Legacy.World.Geometry;

public enum GeometryRenderMode
{
    // Only render dynamic geometry (moving sectors, scrolling floors etc). Default.
    Dynamic,
    // Render all geometry even if it's static.
    All
}

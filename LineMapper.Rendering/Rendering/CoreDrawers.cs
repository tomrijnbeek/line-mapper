using Bearded.Graphics;
using Bearded.Graphics.Shapes;
using LineMapper.Rendering.Drawers;

namespace LineMapper.Rendering.Rendering;

public sealed class CoreDrawers
{
    public IShapeDrawer2<Color> Primitives { get; }
    public IArcDrawer<Color> Arcs { get; }

    public CoreDrawers(CoreRenderers renderers)
    {
        Primitives = new ShapeDrawer2<ColorVertexData, Color>(
            renderers.Primitives,
            (xyz, color) => new ColorVertexData(xyz, color));
        Arcs = new ArcDrawer<ColorVertexData, Color>(
            renderers.Primitives,
            (xyz, color) => new ColorVertexData(xyz, color));
        ;        }
}

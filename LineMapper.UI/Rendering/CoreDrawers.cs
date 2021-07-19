using Bearded.Graphics;
using Bearded.Graphics.Shapes;

namespace LineMapper.UI.Rendering
{
    public sealed class CoreDrawers
    {
        public IShapeDrawer2<Color> Primitives { get; }

        public CoreDrawers(CoreRenderers renderers)
        {
            Primitives = new ShapeDrawer2<ColorVertexData, Color>(
                renderers.Primitives,
                (xyz, color) => new ColorVertexData(xyz, color));
        }
    }
}

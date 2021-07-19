using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.Shapes;
using LineMapper.Rendering.Shaders;

namespace LineMapper.Rendering.Rendering
{
    public sealed class CoreRenderers
    {
        public ExpandingIndexedTrianglesMeshBuilder<ColorVertexData> Primitives { get; } = new();
        public IRenderer PrimitivesRenderer { get; }

        public CoreRenderers(CoreShaders shaders)
        {
            var geometryShader = shaders.GetRendererShader("geometry");

            PrimitivesRenderer = BatchedRenderer.From(Primitives.ToRenderable());
            geometryShader.UseOnRenderer(PrimitivesRenderer);
        }
    }
}

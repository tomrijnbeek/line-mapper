using Bearded.UI.Rendering;
using LineMapper.Rendering.Drawers;
using LineMapper.Rendering.Rendering;
using LineMapper.UI.Controls;

namespace LineMapper.UI.Renderers
{
    sealed class LayoutControlRenderer : IRenderer<LayoutControl>
    {
        private readonly LineDrawer lineDrawer;

        public LayoutControlRenderer(RenderContext renderContext)
        {
            lineDrawer = new LineDrawer(renderContext.Drawers);
        }

        public void Render(LayoutControl control)
        {
            foreach (var line in control.LaidOutLines)
            {
                lineDrawer.DrawLine(line);
            }
        }
    }
}

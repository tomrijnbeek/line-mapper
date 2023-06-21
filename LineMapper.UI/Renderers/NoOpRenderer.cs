using Bearded.UI.Controls;
using Bearded.UI.Rendering;

namespace LineMapper.UI.Renderers;

sealed class NoOpRenderer : IRenderer<Control>
{
    public void Render(Control control) {}
}

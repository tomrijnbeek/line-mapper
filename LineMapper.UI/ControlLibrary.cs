using System;
using System.Collections.Immutable;
using Bearded.UI.Controls;
using Bearded.UI.Rendering;
using LineMapper.Rendering.Rendering;
using LineMapper.UI.Controls;
using LineMapper.UI.Renderers;

namespace LineMapper.UI;

public static class ControlLibrary
{
    public static IRendererRouter InitializeRenderers(RenderContext renderContext)
    {
        var renderers = ImmutableArray.Create<(Type type, object renderer)>(
            (typeof(LayoutControl), new LayoutControlRenderer(renderContext)),
            (typeof(Control), new NoOpRenderer())
        );

        return new CachedRendererRouter(renderers);
    }
}

using Bearded.Graphics.RenderSettings;

namespace LineMapper.Rendering.Rendering;

public sealed class SharedRenderSettings
{
    public Matrix4Uniform ViewMatrix { get; } = new("view");
    public Matrix4Uniform ProjectionMatrix { get; } = new("projection");
}

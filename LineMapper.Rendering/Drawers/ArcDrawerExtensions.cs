using OpenTK.Mathematics;

namespace LineMapper.Rendering.Drawers;

static class ArcDrawerExtensions
{
    public static void DrawArc<TVertexParameters>(
        this IArcDrawer<TVertexParameters> arcDrawer,
        Vector2 center,
        float directionStart,
        float directionEnd,
        float radius,
        float lineWidth,
        TVertexParameters parameters,
        int numEdges)
    {
        arcDrawer.DrawArc(
            center.X, center.Y, 0, directionStart, directionEnd, radius, lineWidth, parameters, numEdges);
    }
}

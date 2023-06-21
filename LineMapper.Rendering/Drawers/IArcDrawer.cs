namespace LineMapper.Rendering.Drawers;

public interface IArcDrawer<in TVertexParameters>
{
    public void DrawArc(
        float centerX, float centerY, float centerZ,
        float angleStart, float angleEnd,
        float radius,
        float lineWidth,
        TVertexParameters parameters,
        int edges);
}

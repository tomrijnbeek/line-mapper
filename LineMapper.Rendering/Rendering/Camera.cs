using Bearded.Utilities.IO;
using OpenTK.Mathematics;

namespace LineMapper.Rendering.Rendering;

public sealed class Camera
{
    private readonly Logger logger;
    private readonly float scale = 1;
    private CenterAndSize visibleArea;

    private Matrix4 view;
    private Matrix4 projection;

    public Camera(Logger logger)
    {
        this.logger = logger;
        visibleArea = new CenterAndSize(0, 0, 1280, 720);
        recalculateView();
        recalculateProjection();
    }

    public void ResizeViewport(Vector2i size)
    {
        logger.Trace?.Log($"Resizing viewport to {size}");

        visibleArea = new CenterAndSize(visibleArea.Center, size.ToVector2() / scale);
        recalculateProjection();
    }

    private void recalculateView()
    {
        view = Matrix4.LookAt(
            new Vector3(visibleArea.Center.X, visibleArea.Center.Y, Constants.CameraHeight),
            new Vector3(visibleArea.Center.X, visibleArea.Center.Y, 0),
            Vector3.UnitY);
    }

    private void recalculateProjection()
    {
        projection = Matrix4.CreateOrthographic(
            visibleArea.Size.X, visibleArea.Size.Y, Constants.ZNear, Constants.ZFar);
    }

    public void ApplyTo(SharedRenderSettings settings)
    {
        settings.ViewMatrix.Value = view;
        settings.ProjectionMatrix.Value = projection;
    }

    private readonly struct CenterAndSize
    {
        public Vector2 Center { get; }
        public Vector2 Size { get; }

        public CenterAndSize(float x, float y, float w, float h) : this(new Vector2(x, y), new Vector2(w, h)) {}

        public CenterAndSize(Vector2 center, Vector2 size)
        {
            Center = center;
            Size = size;
        }
    }
}

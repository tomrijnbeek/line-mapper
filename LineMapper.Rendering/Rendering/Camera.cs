using System;
using Bearded.Utilities.IO;
using Bearded.Utilities.SpaceTime;
using OpenTK.Mathematics;

namespace LineMapper.Rendering.Rendering;

public sealed class Camera : IMousePositionTransform
{
    private readonly Logger logger;
    private readonly float scale = 1;
    private Vector2i viewportSize;
    private CenterAndSize visibleArea;

    private Matrix4 view;
    private Matrix4 projection;

    public Camera(Logger logger)
    {
        this.logger = logger;
        viewportSize = new Vector2i(1280, 720);
        visibleArea = new CenterAndSize(0, 0, viewportSize.X, viewportSize.Y);
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

    public Position2 ToWorldPosition(Vector2d mousePosition)
    {
        var projectionInverted = projection.Inverted();
        var viewInverted = view.Inverted();

        var normalizedMousePosition = new Vector3(
            2 * (float) mousePosition.X / viewportSize.X - 1, 2 * (float) mousePosition.Y / viewportSize.Y - 1, 0);
        var unprojectedMousePosition = Vector3.TransformPerspective(normalizedMousePosition, projectionInverted);
        var untransformedMousePosition = Vector3.TransformPosition(unprojectedMousePosition, viewInverted);

        Console.WriteLine(untransformedMousePosition);
        return new Position2(untransformedMousePosition.Xy);
    }

    private readonly record struct CenterAndSize(Vector2 Center, Vector2 Size)
    {
        public CenterAndSize(float x, float y, float w, float h) : this(new Vector2(x, y), new Vector2(w, h)) {}
    }
}

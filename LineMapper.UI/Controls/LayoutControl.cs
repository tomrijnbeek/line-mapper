using System.Collections.Generic;
using System.Collections.Immutable;
using Bearded.Graphics;
using Bearded.UI.Controls;
using Bearded.UI.EventArgs;
using Bearded.UI.Rendering;
using Bearded.Utilities.SpaceTime;
using LineMapper.Model;
using LineMapper.Model.Editor;
using LineMapper.Model.Layout;
using LineMapper.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LineMapper.UI.Controls;

public sealed class LayoutControl : CompositeControl
{
    private readonly IMousePositionTransform mousePositionTransform;
    private readonly DynamicLayout layout;

    private IPointHandle? draggingPoint;

    public IEnumerable<Point> Points => layout.Points;
    public IEnumerable<LaidOutLine> LaidOutLines => layout.LaidOutLines;

    public LayoutControl(IMousePositionTransform mousePositionTransform)
    {
        this.mousePositionTransform = mousePositionTransform;
        var layoutBuilder = new LimitedDirectionLayoutBuilder(LimitedDirectionLayoutBuilder.OctagonalDirections);
        layout = new DynamicLayout(layoutBuilder);

        var a1 = layout.AddPoint("A1", new Position2(-400, -300));
        var a2 = layout.AddPoint("A2", new Position2(-300, 0));
        var a3 = layout.AddPoint("A3", new Position2(0, 0));
        var a4 = layout.AddPoint("A4", new Position2(100, 100));
        var a5 = layout.AddPoint("A5", new Position2(400, 100));
        var b1 = layout.AddPoint("B1", new Position2(-300, 300));
        var b2 = layout.AddPoint("B2", new Position2(-50, 50));
        var b3 = layout.AddPoint("B3", new Position2(150, 50));
        var b4 = layout.AddPoint("B4", new Position2(400, -300));
        var c1 = layout.AddPoint("C1", new Position2(-50, 300));
        var c2 = b2;
        var c3 = a3;
        var c4 = b4;
        var d1 = layout.AddPoint("D1", new Position2(400, 300));
        var d2 = a4;
        var d3 = a3;
        var d4 = layout.AddPoint("D4", new Position2(0, -300));

        layout.AddLine("A", Color.Red, ImmutableArray.Create(a1, a2, a3, a4, a5));
        layout.AddLine("B", Color.Blue, ImmutableArray.Create(b1, b2, b3, b4));
        layout.AddLine("C", Color.Green, ImmutableArray.Create(c1, c2, c3, c4));
        layout.AddLine("D", Color.HotPink, ImmutableArray.Create(d1, d2, d3, d4));
    }

    public override void MouseButtonHit(MouseButtonEventArgs eventArgs)
    {
        if (eventArgs.MouseButton == MouseButton.Left &&
            draggingPoint is null &&
            layout.TryFindPoint(mouseWorldPosition(eventArgs), out var point))
        {
            draggingPoint = point;
            eventArgs.Handled = true;
            return;
        }

        base.MouseButtonHit(eventArgs);
    }

    public override void MouseMoved(MouseEventArgs eventArgs)
    {
        if (draggingPoint is { } point)
        {
            point.MoveTo(mouseWorldPosition(eventArgs));
            eventArgs.Handled = true;
            return;
        }

        base.MouseMoved(eventArgs);
    }

    public override void MouseButtonReleased(MouseButtonEventArgs eventArgs)
    {
        if (eventArgs.MouseButton == MouseButton.Left && draggingPoint is { } point)
        {
            point.MoveTo(mouseWorldPosition(eventArgs));
            draggingPoint = null;
            eventArgs.Handled = true;
            return;
        }

        base.MouseButtonReleased(eventArgs);
    }

    private Position2 mouseWorldPosition<T>(T eventArgs) where T : MouseEventArgs =>
        mousePositionTransform.ToWorldPosition(eventArgs.MousePosition);

    protected override void RenderStronglyTyped(IRendererRouter r) => r.Render(this);
}

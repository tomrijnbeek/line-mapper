using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bearded.Graphics;
using Bearded.Utilities.SpaceTime;
using LineMapper.Model.Layout;

namespace LineMapper.Model.Editor;

public sealed class DynamicLayout
{
    private readonly ILayoutBuilder layoutBuilder;
    private readonly List<PointHandle> points = new();
    private readonly List<LineHandle> lines = new();

    public IEnumerable<Point> Points => points.Select(p => new Point(p.Name, p.Position));
    public ImmutableArray<LaidOutLine> LaidOutLines { get; private set; }

    public DynamicLayout(ILayoutBuilder layoutBuilder)
    {
        this.layoutBuilder = layoutBuilder;
    }

    public bool TryFindPoint(Position2 position, [NotNullWhen(true)] out IPointHandle? point)
    {
        var rSquared = Constants.StationRadius.Squared;
        foreach (var p in points)
        {
            var d = position - p.Position;
            var dSquared = d.LengthSquared;
            if (dSquared <= rSquared)
            {
                point = p;
                return true;
            }
        }

        point = default;
        return false;
    }

    public IPointHandle AddPoint(string name, Position2 position)
    {
        var handle = new PointHandle(this, name, position);
        points.Add(handle);
        return handle;
    }

    public ILineHandle AddLine(string name, Color color, ImmutableArray<IPointHandle> points)
    {
        var handle = new LineHandle(name, color, points.CastArray<INode>());
        lines.Add(handle);
        updateLayout();
        return handle;
    }

    private void updateLayout()
    {
        LaidOutLines = layoutBuilder.LayOutLines(lines.Select(l => l.Line));
    }

    private sealed class PointHandle : IPointHandle
    {
        private readonly DynamicLayout layout;
        public string Name { get; }
        public Position2 Position { get; private set; }

        public PointHandle(DynamicLayout layout, string name, Position2 position)
        {
            this.layout = layout;
            Name = name;
            Position = position;
        }

        public void MoveTo(Position2 newPosition)
        {
            Position = newPosition;
            layout.updateLayout();
        }
    }

    private sealed class LineHandle : ILineHandle
    {
        public Line Line { get; }

        public LineHandle(string name, Color color, ImmutableArray<INode> nodes)
        {
            Line = new Line(name, color, nodes);
        }
    }
}

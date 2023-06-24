using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Bearded.Utilities.SpaceTime;

namespace LineMapper.Model.Layout;

public sealed class DynamicLayout
{
    private readonly ILayoutBuilder layoutBuilder;
    private readonly List<Point> points = new();
    private readonly List<Line> lines = new();

    public IEnumerable<Point> Points { get; }
    public ImmutableArray<LaidOutLine> LaidOutLines { get; private set; }

    public DynamicLayout(ILayoutBuilder layoutBuilder)
    {
        this.layoutBuilder = layoutBuilder;
        Points = points.AsReadOnly();
    }

    public bool TryFindPoint(Position2 position, [NotNullWhen(true)] out Point? point)
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

    public void AddPoint(Point point)
    {
        points.Add(point);
    }

    public void AddLine(Line line)
    {
        lines.Add(line);
        updateLayout();
    }

    public void MovePoint(Point point, Position2 newPosition)
    {
        var index = points.IndexOf(point);
        if (index < 0)
        {
            return;
        }

        points[index] = point with { Position = newPosition };
        updateLayout();
    }

    private void updateLayout()
    {
        LaidOutLines = layoutBuilder.LayOutLines(lines);
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.SpaceTime;
using OpenTK.Mathematics;

namespace LineMapper.Model.Layout;

public sealed partial class LimitedDirectionLayoutBuilder : ILayoutBuilder
{
    public static readonly ImmutableArray<Direction2> OctagonalDirections =
        Enumerable.Range(0, 8).Select(i => Direction2.FromDegrees(i * 45)).ToImmutableArray();
    public static readonly ImmutableArray<Direction2> HexagonalDirections =
        Enumerable.Range(0, 6).Select(i => Direction2.FromDegrees(i * 60)).ToImmutableArray();

    private static readonly Angle epsilonAngle = Angle.FromRadians(0.01f);

    private readonly ImmutableArray<Direction2> directions;

    public LimitedDirectionLayoutBuilder(IEnumerable<Direction2> directions)
    {
        this.directions = directions.OrderBy(d => d.Degrees).ToImmutableArray();
    }

    // TODO: return an intermediate result that allows for partial recalculation
    public ImmutableArray<LaidOutLine> LayOutLines(IEnumerable<Line> lines)
    {
        return lines.Select(layOutLine).ToImmutableArray();
    }

    private LaidOutLine layOutLine(Line line) =>
        new(line.Color, createLineSegments(line).ToImmutableArray());

    private IEnumerable<LineSegment> createLineSegments(Line line)
    {
        var nodes = line.Nodes;
        var sections = breakIntoSections(nodes);
        var sectionDirections = findSectionDirections(sections);
        sectionDirections = optimizeSectionDirections(sectionDirections);
        return toLineSegments(sections, sectionDirections);
    }

    private ImmutableArray<Section> breakIntoSections(ImmutableArray<INode> nodes)
    {
        var result = ImmutableArray.CreateBuilder<Section>(nodes.Length - 1);
        for (var i = 0; i < nodes.Length - 1; i++)
        {
            result.Add(new Section(nodes[i].Position, nodes[i + 1].Position));
        }
        return result.MoveToImmutable();
    }

    private readonly record struct Section(Position2 Start, Position2 End)
    {
        public Difference2 Difference => End - Start;
    }
}

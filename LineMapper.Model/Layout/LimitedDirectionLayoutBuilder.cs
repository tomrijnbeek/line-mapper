using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.SpaceTime;
using OpenTK.Mathematics;

namespace LineMapper.Model.Layout;

public sealed class LimitedDirectionLayoutBuilder : ILayoutBuilder
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
        for (var i = 0; i < nodes.Length - 1; i++)
        {
            var isFirstSegment = i == 0;
            var isLastSegment = i == nodes.Length - 2;

            var start = nodes[i].Position;
            var end = nodes[i + 1].Position;
            var difference = end - start;
            var direction = difference.Direction;

            var (dirBefore, dirAfter) = findDirectionBucket(direction);

            // If we already align with an allowed direction, just draw a straight segment.
            if (direction - dirBefore < epsilonAngle || dirAfter - direction < epsilonAngle)
            {
                yield return createLineSegment(start, end, isFirstSegment, isLastSegment);
                continue;
            }

            // TODO: there are few things we can cache here
            var basisX = dirBefore.Vector;
            var basisY = dirAfter.Vector;

            var basisTransform = new Matrix2(basisX, basisY);
            basisTransform.Invert();

            var differenceInBasis = difference.NumericValue * basisTransform;

            var bendOffset = new Difference2(dirBefore.Vector * differenceInBasis.X);
            var bendPosition = start + bendOffset;

            yield return createLineSegment(start, bendPosition, isFirstSegment, false);
            yield return createLineSegment(bendPosition, end, false, isLastSegment);
        }
    }

    private (Direction2 Before, Direction2 After) findDirectionBucket(Direction2 exactDirection)
    {
        for (var i = 0; i < directions.Length; i++)
        {
            var dirBefore = directions[i];
            var dirAfter = directions[(i + 1) % directions.Length];

            if (exactDirection - dirBefore >= Angle.Zero && dirAfter - exactDirection > Angle.Zero)
            {
                return (dirBefore, dirAfter);
            }
        }

        throw new Exception();
    }

    private static LineSegment createLineSegment(
        Position2 start, Position2 end, bool removeArcStart, bool removeArcEnd)
    {
        var difference = end - start;
        var offsetForArc = difference * (Constants.ArcRadius / difference.Length);
        var segmentStart = removeArcStart ? start : start + offsetForArc;
        var segmentEnd = removeArcEnd ? end : end - offsetForArc;

        return new LineSegment(segmentStart, segmentEnd);
    }
}

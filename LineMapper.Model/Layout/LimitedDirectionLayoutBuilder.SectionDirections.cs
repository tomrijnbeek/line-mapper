using System;
using System.Collections.Immutable;
using Bearded.Utilities.Geometry;

namespace LineMapper.Model.Layout;

public sealed partial class LimitedDirectionLayoutBuilder
{
    private ImmutableArray<SectionDirections> findSectionDirections(ImmutableArray<Section> sections)
    {
        var result = ImmutableArray.CreateBuilder<SectionDirections>(sections.Length);
        foreach (var section in sections)
        {
            var direction = section.Difference.Direction;

            var (dirBefore, dirAfter) = findDirectionBucket(direction);

            // If we already align with an allowed direction, just draw a straight segment.
            if (direction - dirBefore < epsilonAngle)
            {
                result.Add(new SectionDirections(dirBefore, null));
            }
            else if (dirAfter - direction < epsilonAngle)
            {
                result.Add(new SectionDirections(dirAfter, null));
            }
            // Otherwise return two directions we can use to build the segment.
            else
            {
                result.Add(new SectionDirections(dirBefore, dirAfter));
            }
        }

        return result.MoveToImmutable();
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

    private ImmutableArray<SectionDirections> optimizeSectionDirections(
        ImmutableArray<SectionDirections> initialDirections)
    {
        // TODO: right now it is an all or nothing between checking forward or backward, but ideally we want a mix of
        //       the two. Perhaps the better approach is for each step to consider both forward and backward bends?
        var frontToBack = optimizeSectionDirections(
            initialDirections, 0, 1, out var frontToBackBendCount);
        var backToFront = optimizeSectionDirections(
            initialDirections, initialDirections.Length - 1, -1, out var backToFrontBendCount);

        return frontToBackBendCount <= backToFrontBendCount ? frontToBack : backToFront;
    }

    private ImmutableArray<SectionDirections> optimizeSectionDirections(
        ImmutableArray<SectionDirections> initialDirections, int startIndex, int dIndex, out int bendCount)
    {
        var result = new SectionDirections[initialDirections.Length];
        bendCount = 0;
        for (var i = startIndex; i >= 0 && i < initialDirections.Length; i += dIndex)
        {
            var current = initialDirections[i];
            if (current.SecondDirection is not null)
            {
                bendCount++;
            }
            if (i == startIndex)
            {
                result[i] = current;
                continue;
            }

            var prev = initialDirections[i - dIndex];
            var incoming = prev.SecondDirection ?? prev.FirstDirection;
            var secondDirBetter = current.SecondDirection is { } candidate &&
                (candidate - incoming).MagnitudeInDegrees < (current.FirstDirection - incoming).MagnitudeInDegrees;
            result[i] = secondDirBetter ? current.Swapped : current;
            if (result[i].FirstDirection != incoming)
            {
                bendCount++;
            }
        }

        return result.ToImmutableArray();
    }

    private readonly record struct SectionDirections(Direction2 FirstDirection, Direction2? SecondDirection)
    {
        public SectionDirections Swapped
        {
            get
            {
                if (SecondDirection is null)
                {
                    throw new InvalidOperationException();
                }
                return new SectionDirections(SecondDirection.Value, FirstDirection);
            }
        }
    }
}

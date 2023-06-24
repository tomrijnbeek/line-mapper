using System.Collections.Generic;
using System.Collections.Immutable;
using Bearded.Utilities.SpaceTime;
using OpenTK.Mathematics;

namespace LineMapper.Model.Layout;

public sealed partial class LimitedDirectionLayoutBuilder
{
    private static IEnumerable<LineSegment> toLineSegments(
        ImmutableArray<Section> sections, ImmutableArray<SectionDirections> sectionDirections)
    {
        for (var i = 0; i < sections.Length; i++)
        {
            var section = sections[i];
            var dirs = sectionDirections[i];
            var isFirstSection = i == 0;
            var isLastSection = i == sections.Length - 1;

            if (dirs.SecondDirection is null)
            {
                yield return createLineSegment(section.Start, section.End, isFirstSection, isLastSection);
                continue;
            }

            // TODO: there are few things we can cache here
            var basisX = dirs.FirstDirection.Vector;
            var basisY = dirs.SecondDirection.Value.Vector;

            var basisTransform = new Matrix2(basisX, basisY);
            basisTransform.Invert();

            var differenceInBasis = section.Difference.NumericValue * basisTransform;

            var bendOffset = new Difference2(dirs.FirstDirection.Vector * differenceInBasis.X);
            var bendPosition = section.Start + bendOffset;

            yield return createLineSegment(section.Start, bendPosition, isFirstSection, false);
            yield return createLineSegment(bendPosition, section.End, false, isLastSection);
        }
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

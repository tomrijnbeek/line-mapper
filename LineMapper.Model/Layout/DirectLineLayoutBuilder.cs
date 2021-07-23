using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Utilities.SpaceTime;

namespace LineMapper.Model.Layout
{
    public sealed class DirectLineLayoutBuilder : ILayoutBuilder
    {
        private static readonly Unit arcRadius = (3 * Constants.LineWidth).U();

        // TODO: return an intermediate result that allows for partial recalculation
        public ImmutableArray<LaidOutLine> LayOutLines(IEnumerable<Line> lines)
        {
            return lines.Select(layOutLine).ToImmutableArray();
        }

        private static LaidOutLine layOutLine(Line line) =>
            new(line.Color, createLineSegments(line).ToImmutableArray());

        private static IEnumerable<LineSegment> createLineSegments(Line line)
        {
            var nodes = line.Nodes;
            for (var i = 0; i < nodes.Length - 1; i++)
            {
                var isFirstSegment = i == 0;
                var isLastSegment = i == nodes.Length - 2;

                var start = nodes[i].Position;
                var end = nodes[i + 1].Position;
                var difference = end - start;
                var offsetForArc = difference * (Constants.StationRadius / difference.Length);

                var segmentStart = isFirstSegment ? start : start + offsetForArc;
                var segmentEnd = isLastSegment ? end : end - offsetForArc;

                // TODO: this will give weird results if the line segment is shorter than 2 * arcRadius
                yield return new LineSegment(start + offsetForArc, end - offsetForArc);
            }
        }
    }
}

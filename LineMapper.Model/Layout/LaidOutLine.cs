using System.Collections.Immutable;

namespace LineMapper.Model.Layout
{
    // TODO: add color
    public sealed record LaidOutLine(ImmutableArray<LineSegment> Segments);
}

using System.Collections.Immutable;
using Bearded.Graphics;

namespace LineMapper.Model.Layout
{
    public sealed record LaidOutLine(Color Color, ImmutableArray<LineSegment> Segments);
}

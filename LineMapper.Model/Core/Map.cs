using System.Collections.Generic;
using System.Collections.Immutable;

namespace LineMapper.Model
{
    public sealed class Map
    {
        private readonly List<Point> points = new();
        private readonly List<Line> line = new();

        public record PointWithAdjacencies(Point Point, ImmutableArray<PointAdjacency> Adjacencies);

        public record PointAdjacency(Point Opposite, Line Line);
    }
}

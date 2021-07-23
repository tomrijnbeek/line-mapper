using System.Linq;
using Bearded.Graphics.Shapes;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.SpaceTime;
using LineMapper.Model.Layout;
using LineMapper.Rendering.Helpers;
using LineMapper.Rendering.Rendering;
using OpenTK.Mathematics;

namespace LineMapper.Rendering.Drawers
{
    public sealed class LineDrawer
    {
        private readonly CoreDrawers coreDrawers;

        public LineDrawer(CoreDrawers coreDrawers)
        {
            this.coreDrawers = coreDrawers;
        }

        public void DrawLine(LaidOutLine line)
        {
            foreach (var segment in line.Segments)
            {
                coreDrawers.Primitives.DrawLine(
                    segment.Start.NumericValue, segment.End.NumericValue, Model.Constants.LineWidth, line.Color);
            }

            foreach (var (to, from) in line.Segments.Skip(1).Zip(line.Segments))
            {
                if (!findArcCenter(from, to, out var center))
                {
                    coreDrawers.Primitives.DrawLine(
                        from.End.NumericValue, to.Start.NumericValue, Model.Constants.LineWidth, line.Color);
                    return;
                }

                var spokeFrom = from.End - center;
                var spokeTo = to.Start - center;

                coreDrawers.Arcs.DrawArc(
                    center.NumericValue,
                    Direction2.Of(spokeFrom.NumericValue).Radians,
                    Direction2.Of(spokeTo.NumericValue).Radians,
                    spokeFrom.Length.NumericValue + 0.5f * Model.Constants.LineWidth,
                    Model.Constants.LineWidth, line.Color, Constants.NumArcSections);
            }
        }

        private static bool findArcCenter(LineSegment from, LineSegment to, out Position2 center)
        {
            var fromDirection = from.Direction;
            var toDirection = to.Direction;

            var difference = fromDirection - toDirection;

            if (difference > Angle.Zero)
            {
                var r1 = Ray2.WithoutNormalizing(from.End.NumericValue, fromDirection.Vector.PerpendicularRight);
                var r2 = Ray2.WithoutNormalizing(to.Start.NumericValue, toDirection.Vector.PerpendicularRight);

                if (Intersect2.Intersect(r1, r2, out var i))
                {
                    center = new Position2(i);
                    return true;
                }
            }
            else
            {
                var r1 = Ray2.WithoutNormalizing(from.End.NumericValue, fromDirection.Vector.PerpendicularLeft);
                var r2 = Ray2.WithoutNormalizing(to.Start.NumericValue, toDirection.Vector.PerpendicularLeft);

                if (Intersect2.Intersect(r1, r2, out var i))
                {
                    center = new Position2(i);
                    return true;
                }
            }

            center = Position2.Zero;
            return false;
        }
    }
}

using Bearded.Utilities.Geometry;
using Bearded.Utilities.SpaceTime;

namespace LineMapper.Model.Layout;

public sealed record LineSegment(Position2 Start, Position2 End)
{
    public Direction2 Direction => (End - Start).Direction;
}

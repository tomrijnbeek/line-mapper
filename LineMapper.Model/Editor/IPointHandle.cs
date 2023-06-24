using Bearded.Utilities.SpaceTime;

namespace LineMapper.Model.Editor;

public interface IPointHandle : INode
{
    void MoveTo(Position2 newPosition);
}

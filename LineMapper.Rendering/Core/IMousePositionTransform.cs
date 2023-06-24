using Bearded.Utilities.SpaceTime;
using OpenTK.Mathematics;

namespace LineMapper.Rendering;

public interface IMousePositionTransform
{
    Position2 ToWorldPosition(Vector2d mousePosition);
}

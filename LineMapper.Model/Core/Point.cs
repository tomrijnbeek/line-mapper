using Bearded.Utilities.SpaceTime;

namespace LineMapper.Model;

public sealed record Point(string Name, Position2 Position) : INode;

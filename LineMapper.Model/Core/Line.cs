using System.Collections.Immutable;

namespace LineMapper.Model
{
    // TODO: add color
    public sealed record Line(string Name, ImmutableArray<INode> Nodes);
}

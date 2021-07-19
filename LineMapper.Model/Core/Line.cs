using System.Collections.Immutable;
using Bearded.Graphics;

namespace LineMapper.Model
{
    public sealed record Line(string Name, Color Color, ImmutableArray<INode> Nodes);
}

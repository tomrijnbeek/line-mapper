using System.Collections.Generic;
using System.Collections.Immutable;

namespace LineMapper.Model.Layout;

public interface ILayoutBuilder
{
    ImmutableArray<LaidOutLine> LayOutLines(IEnumerable<Line> lines);
}

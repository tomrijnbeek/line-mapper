using System.Collections.Generic;
using System.Collections.Immutable;

namespace LineMapper.Model.Layout;

interface ILayoutBuilder
{
    ImmutableArray<LaidOutLine> LayOutLines(IEnumerable<Line> lines);
}

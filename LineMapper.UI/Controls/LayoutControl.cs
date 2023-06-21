using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Graphics;
using Bearded.UI.Controls;
using Bearded.UI.Rendering;
using Bearded.Utilities.SpaceTime;
using LineMapper.Model;
using LineMapper.Model.Layout;

namespace LineMapper.UI.Controls;

public sealed class LayoutControl : CompositeControl
{
    public IEnumerable<Point> Points { get; }
    public IEnumerable<LaidOutLine> LaidOutLines { get; }

    public LayoutControl()
    {
        var a1 = new Point("A1", new Position2(-400, -300));
        var a2 = new Point("A2", new Position2(-300, 0));
        var a3 = new Point("A3", new Position2(0, 0));
        var a4 = new Point("A4", new Position2(100, 100));
        var a5 = new Point("A5", new Position2(400, 100));
        var b1 = new Point("B1", new Position2(-300, 300));
        var b2 = new Point("B2", new Position2(-50, 50));
        var b3 = new Point("B3", new Position2(150, 50));
        var b4 = new Point("B4", new Position2(400, -300));
        var c1 = new Point("C1", new Position2(-50, 300));
        var c2 = b2;
        var c3 = a3;
        var c4 = b4;
        var d1 = new Point("D1", new Position2(400, 300));
        var d2 = a4;
        var d3 = a3;
        var d4 = new Point("D4", new Position2(0, -300));

        Points = ImmutableArray
            .Create(a1, a2, a3, a4, a5, b1, b2, b3, b4, c1, c2, c3, c4, d1, d2, d3, d4)
            .Distinct()
            .ToImmutableArray();

        var aLine = new Line("A", Color.Red, ImmutableArray.Create<INode>(a1, a2, a3, a4, a5));
        var bLine = new Line("B", Color.Blue, ImmutableArray.Create<INode>(b1, b2, b3, b4));
        var cLine = new Line("C", Color.Green, ImmutableArray.Create<INode>(c1, c2, c3, c4));
        var dLine = new Line("D", Color.HotPink, ImmutableArray.Create<INode>(d1, d2, d3, d4));

        var lines = ImmutableArray.Create(aLine, bLine, cLine, dLine);

        var layoutBuilder = new LimitedDirectionLayoutBuilder(LimitedDirectionLayoutBuilder.OctagonalDirections);
        LaidOutLines = layoutBuilder.LayOutLines(lines);
    }

    protected override void RenderStronglyTyped(IRendererRouter r) => r.Render(this);
}

using System.Collections.Generic;
using System.Collections.Immutable;
using Bearded.Graphics;
using Bearded.UI.Controls;
using Bearded.UI.Rendering;
using Bearded.Utilities.SpaceTime;
using LineMapper.Model.Layout;

namespace LineMapper.UI.Controls
{
    public sealed class LayoutControl : CompositeControl
    {
        public IEnumerable<LaidOutLine> LaidOutLines { get; }

        public LayoutControl()
        {
            LaidOutLines = ImmutableArray.Create(
                new LaidOutLine(Color.Red, ImmutableArray.Create(
                    new LineSegment(new Position2(-200, -200), new Position2(-200, 0)),
                    new LineSegment(new Position2(-200, 0), new Position2(0, 0)),
                    new LineSegment(new Position2(0, 0), new Position2(100, 100)))),
                new LaidOutLine(Color.Blue, ImmutableArray.Create(
                    new LineSegment(new Position2(-200, 200), new Position2(-50, 50)),
                    new LineSegment(new Position2(-50, 50), new Position2(150, 50)),
                    new LineSegment(new Position2(150, 50), new Position2(150, -200))))
                );
        }

        protected override void RenderStronglyTyped(IRendererRouter r) => r.Render(this);
    }
}

using Bearded.Graphics.Shapes;
using LineMapper.Model.Layout;
using LineMapper.Rendering.Rendering;

namespace LineMapper.Rendering.Drawers
{
    sealed class LineDrawer
    {
        private readonly CoreDrawers coreDrawers;

        public LineDrawer(CoreDrawers coreDrawers)
        {
            this.coreDrawers = coreDrawers;
        }

        public void DrawLine(LaidOutLine line)
        {
            foreach (var segment in line.Segments)
            {
                coreDrawers.Primitives.DrawLine(
                    segment.Start.NumericValue, segment.End.NumericValue, Model.Constants.LineWidth, line.Color);
            }
        }
    }
}

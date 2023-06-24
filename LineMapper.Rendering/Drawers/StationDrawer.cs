using Bearded.Graphics;
using Bearded.Graphics.Shapes;
using LineMapper.Rendering.Rendering;
using Point = LineMapper.Model.Point;

namespace LineMapper.Rendering.Drawers;

public sealed class StationDrawer
{
    private readonly CoreDrawers coreDrawers;

    public StationDrawer(CoreDrawers coreDrawers)
    {
        this.coreDrawers = coreDrawers;
    }

    public void DrawStation(Point point, bool isActive)
    {
        coreDrawers.Primitives.FillCircle(
            point.Position.NumericValue, Model.Constants.StationRadius.NumericValue, Color.Black);
        coreDrawers.Primitives.FillCircle(
            point.Position.NumericValue, Model.Constants.InnerStationRadius.NumericValue, isActive ? Color.Yellow : Color.White);
    }
}

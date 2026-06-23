using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Factories;

public class LineShapeFactory : IShapeFactory
{
    public string DisplayName => "Line";

    public Shape Create(Rectangle bounds, Color color, float thickness)
    {
        return new LineShape(bounds.Location, new Point(bounds.Right, bounds.Bottom), color, thickness);
    }
}

using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Factories;

public class CircleShapeFactory : IShapeFactory
{
    public string DisplayName => "Circle";

    public Shape Create(Rectangle bounds, Color color, float thickness)
    {
        int radius = Math.Min(bounds.Width, bounds.Height) / 2;
        var center = new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
        return new CircleShape(center, radius, color, thickness);
    }
}

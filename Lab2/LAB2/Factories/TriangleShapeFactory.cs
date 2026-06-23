using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Factories;

public class TriangleShapeFactory : IShapeFactory
{
    public string DisplayName => "Triangle";

    public Shape Create(Rectangle bounds, Color color, float thickness)
    {
        Point top = new(bounds.Left + bounds.Width / 2, bounds.Top);
        Point left = new(bounds.Left, bounds.Bottom);
        Point right = new(bounds.Right, bounds.Bottom);
        return new TriangleShape(top, left, right, color, thickness);
    }
}

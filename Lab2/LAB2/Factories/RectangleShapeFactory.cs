using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Factories;

public class RectangleShapeFactory : IShapeFactory
{
    public string DisplayName => "Rectangle";

    public Shape Create(Rectangle bounds, Color color, float thickness)
    {
        return new RectangleShape(bounds, color, thickness);
    }
}

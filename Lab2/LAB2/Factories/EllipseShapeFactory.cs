using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Factories;

public class EllipseShapeFactory : IShapeFactory
{
    public string DisplayName => "Ellipse";

    public Shape Create(Rectangle bounds, Color color, float thickness)
    {
        return new EllipseShape(bounds, color, thickness);
    }
}

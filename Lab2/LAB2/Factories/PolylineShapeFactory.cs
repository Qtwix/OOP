using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Factories;

public class PolylineShapeFactory : IShapeFactory
{
    public string DisplayName => "Polyline";

    public Shape Create(Rectangle bounds, Color color, float thickness)
    {
        Point[] points =
        [
            new Point(bounds.Left, bounds.Top),
            new Point(bounds.Left + bounds.Width / 3, bounds.Top + bounds.Height / 2),
            new Point(bounds.Left + 2 * bounds.Width / 3, bounds.Top + bounds.Height / 4),
            new Point(bounds.Right, bounds.Bottom)
        ];

        return new PolylineShape(points, color, thickness);
    }
}

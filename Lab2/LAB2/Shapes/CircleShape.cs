namespace ShapesDrawingApp.Shapes;

public class CircleShape : Shape
{
    public CircleShape(Point center, int radius, Color color, float thickness)
        : base(color, thickness)
    {
        Center = center;
        Radius = radius;
    }

    public Point Center { get; }

    public int Radius { get; }
}

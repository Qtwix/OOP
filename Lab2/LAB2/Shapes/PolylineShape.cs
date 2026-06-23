namespace ShapesDrawingApp.Shapes;

public class PolylineShape : Shape
{
    public PolylineShape(Point[] points, Color color, float thickness)
        : base(color, thickness)
    {
        Points = points;
    }

    public Point[] Points { get; }
}

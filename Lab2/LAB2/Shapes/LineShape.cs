namespace ShapesDrawingApp.Shapes;

public class LineShape : Shape
{
    public LineShape(Point start, Point end, Color color, float thickness)
        : base(color, thickness)
    {
        Start = start;
        End = end;
    }

    public Point Start { get; }

    public Point End { get; }
}

namespace ShapesDrawingApp.Shapes;

public class TriangleShape : Shape
{
    public TriangleShape(Point first, Point second, Point third, Color color, float thickness)
        : base(color, thickness)
    {
        First = first;
        Second = second;
        Third = third;
    }

    public Point First { get; }

    public Point Second { get; }

    public Point Third { get; }
}

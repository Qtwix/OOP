namespace ShapesDrawingApp.Shapes;

public class TriangleShape : Shape
{
    private readonly Point _first;
    private readonly Point _second;
    private readonly Point _third;

    public TriangleShape(Point first, Point second, Point third, Color color, float thickness)
        : base(color, thickness)
    {
        _first = first;
        _second = second;
        _third = third;
    }

    public override void Draw(Graphics graphics)
    {
        using Pen pen = new(Color, Thickness);
        graphics.DrawPolygon(pen, new Point[] { _first, _second, _third });
    }
}

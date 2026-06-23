namespace ShapesDrawingApp.Shapes;

public class LineShape : Shape
{
    private readonly Point _start;
    private readonly Point _end;

    public LineShape(Point start, Point end, Color color, float thickness)
        : base(color, thickness)
    {
        _start = start;
        _end = end;
    }

    public override void Draw(Graphics graphics)
    {
        using Pen pen = new(Color, Thickness);
        graphics.DrawLine(pen, _start, _end);
    }
}

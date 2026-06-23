namespace ShapesDrawingApp.Shapes;

public class PolylineShape : Shape
{
    private readonly Point[] _points;

    public PolylineShape(Point[] points, Color color, float thickness)
        : base(color, thickness)
    {
        _points = points;
    }

    public override void Draw(Graphics graphics)
    {
        if (_points.Length < 2)
        {
            return;
        }

        using Pen pen = new(Color, Thickness);
        graphics.DrawLines(pen, _points);
    }
}

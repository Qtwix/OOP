namespace ShapesDrawingApp.Shapes;

public class CircleShape : Shape
{
    private readonly Point _center;
    private readonly int _radius;

    public CircleShape(Point center, int radius, Color color, float thickness)
        : base(color, thickness)
    {
        _center = center;
        _radius = radius;
    }

    public override void Draw(Graphics graphics)
    {
        using Pen pen = new(Color, Thickness);
        int diameter = _radius * 2;
        var bounds = new Rectangle(_center.X - _radius, _center.Y - _radius, diameter, diameter);
        graphics.DrawEllipse(pen, bounds);
    }
}

namespace ShapesDrawingApp.Shapes;

public class EllipseShape : Shape
{
    private readonly Rectangle _bounds;

    public EllipseShape(Rectangle bounds, Color color, float thickness)
        : base(color, thickness)
    {
        _bounds = bounds;
    }

    public override void Draw(Graphics graphics)
    {
        using Pen pen = new(Color, Thickness);
        graphics.DrawEllipse(pen, _bounds);
    }
}

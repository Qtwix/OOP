namespace ShapesDrawingApp.Shapes;

public class EllipseShape : Shape
{
    public EllipseShape(Rectangle bounds, Color color, float thickness)
        : base(color, thickness)
    {
        Bounds = bounds;
    }

    public Rectangle Bounds { get; }
}

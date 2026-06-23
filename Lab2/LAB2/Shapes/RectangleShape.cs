namespace ShapesDrawingApp.Shapes;

public class RectangleShape : Shape
{
    public RectangleShape(Rectangle rectangle, Color color, float thickness)
        : base(color, thickness)
    {
        Bounds = rectangle;
    }

    public Rectangle Bounds { get; }
}

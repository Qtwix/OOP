namespace ShapesDrawingApp.Shapes;

public abstract class Shape
{
    protected Shape(Color color, float thickness)
    {
        Color = color;
        Thickness = thickness;
    }

    protected Color Color { get; }
    
    protected float Thickness { get; }

    public abstract void Draw(Graphics graphics);
}

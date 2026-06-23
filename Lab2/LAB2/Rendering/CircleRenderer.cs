using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public class CircleRenderer : IShapeRenderer
{
    public Type ShapeType => typeof(CircleShape);

    public void Draw(Graphics graphics, Shape shape)
    {
        var circle = (CircleShape)shape;
        using Pen pen = new(circle.StrokeColor, circle.StrokeThickness);
        int diameter = circle.Radius * 2;
        Rectangle bounds = new(
            circle.Center.X - circle.Radius,
            circle.Center.Y - circle.Radius,
            diameter,
            diameter);
        graphics.DrawEllipse(pen, bounds);
    }
}

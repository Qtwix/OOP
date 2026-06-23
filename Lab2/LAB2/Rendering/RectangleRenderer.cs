using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public class RectangleRenderer : IShapeRenderer
{
    public Type ShapeType => typeof(RectangleShape);

    public void Draw(Graphics graphics, Shape shape)
    {
        var rectangle = (RectangleShape)shape;
        using Pen pen = new(rectangle.StrokeColor, rectangle.StrokeThickness);
        graphics.DrawRectangle(pen, rectangle.Bounds);
    }
}

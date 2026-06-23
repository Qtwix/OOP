using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public class EllipseRenderer : IShapeRenderer
{
    public Type ShapeType => typeof(EllipseShape);

    public void Draw(Graphics graphics, Shape shape)
    {
        var ellipse = (EllipseShape)shape;
        using Pen pen = new(ellipse.StrokeColor, ellipse.StrokeThickness);
        graphics.DrawEllipse(pen, ellipse.Bounds);
    }
}

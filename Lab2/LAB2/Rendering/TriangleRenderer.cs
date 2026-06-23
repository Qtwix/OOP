using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public class TriangleRenderer : IShapeRenderer
{
    public Type ShapeType => typeof(TriangleShape);

    public void Draw(Graphics graphics, Shape shape)
    {
        var triangle = (TriangleShape)shape;
        using Pen pen = new(triangle.StrokeColor, triangle.StrokeThickness);
        graphics.DrawPolygon(pen, new Point[] { triangle.First, triangle.Second, triangle.Third });
    }
}

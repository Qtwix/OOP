using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public class LineRenderer : IShapeRenderer
{
    public Type ShapeType => typeof(LineShape);

    public void Draw(Graphics graphics, Shape shape)
    {
        var line = (LineShape)shape;
        using Pen pen = new(line.StrokeColor, line.StrokeThickness);
        graphics.DrawLine(pen, line.Start, line.End);
    }
}

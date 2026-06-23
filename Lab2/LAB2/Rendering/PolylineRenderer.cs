using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public class PolylineRenderer : IShapeRenderer
{
    public Type ShapeType => typeof(PolylineShape);

    public void Draw(Graphics graphics, Shape shape)
    {
        var polyline = (PolylineShape)shape;
        if (polyline.Points.Length < 2)
        {
            return;
        }

        using Pen pen = new(polyline.StrokeColor, polyline.StrokeThickness);
        graphics.DrawLines(pen, polyline.Points);
    }
}

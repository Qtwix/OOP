using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public interface IShapeRenderer
{
    Type ShapeType { get; }

    void Draw(Graphics graphics, Shape shape);
}

using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Collections;

public class FigureList
{
    private readonly List<Shape> _shapes = [];

    public void Add(Shape shape)
    {
        _shapes.Add(shape);
    }

    public IReadOnlyList<Shape> Items => _shapes;

    public void Clear()
    {
        _shapes.Clear();
    }
}

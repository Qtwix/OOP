using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Factories;

public interface IShapeFactory
{
    string DisplayName { get; }

    Shape Create(Rectangle bounds, Color color, float thickness);
}

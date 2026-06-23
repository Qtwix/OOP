using System.Reflection;
using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Rendering;

public class ShapeRendererService
{
    private readonly Dictionary<Type, IShapeRenderer> _renderers;

    public ShapeRendererService()
    {
        _renderers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IShapeRenderer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IShapeRenderer)Activator.CreateInstance(t)!)
            .ToDictionary(r => r.ShapeType);
    }

    public void Draw(Graphics graphics, Shape shape)
    {
        if (_renderers.TryGetValue(shape.GetType(), out IShapeRenderer? renderer))
        {
            renderer.Draw(graphics, shape);
        }
    }
}

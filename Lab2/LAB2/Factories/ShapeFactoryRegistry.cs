using System.Reflection;

namespace ShapesDrawingApp.Factories;

public class ShapeFactoryRegistry
{
    private readonly List<IShapeFactory> _factories;

    public ShapeFactoryRegistry()
    {
        _factories = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IShapeFactory).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IShapeFactory)Activator.CreateInstance(t)!)
            .OrderBy(f => f.DisplayName)
            .ToList();
    }

    public IReadOnlyList<IShapeFactory> Factories => _factories;
}

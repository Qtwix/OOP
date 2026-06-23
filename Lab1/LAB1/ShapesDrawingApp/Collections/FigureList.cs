using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp.Collections;

public class FigureList
{
    // ������ ����� 
    private readonly List<Shape> _shapes = [];

    // ����� ��� ���������� ������
    public void Add(Shape shape)
    {
        _shapes.Add(shape);
    }

    // ����� ��� ��������� ���� �����
    public void DrawAll(Graphics graphics)
    {
        foreach (Shape shape in _shapes)
        {
            shape.Draw(graphics);
        }
    }
}

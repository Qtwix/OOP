namespace ShapesDrawingApp.Shapes;

public class RectangleShape : Shape // ��������� ������ Shape
{
    private readonly Rectangle _rectangle;

    // �����������, ����������� ��������� ��� ���������
    public RectangleShape(Rectangle rectangle, Color color, float thickness)
        : base(color, thickness)
    {
        _rectangle = rectangle;
    }

    // ��������������� ������ ���������
    public override void Draw(Graphics graphics)
    {
        using Pen pen = new(Color, Thickness);
        graphics.DrawRectangle(pen, _rectangle);
    }
}

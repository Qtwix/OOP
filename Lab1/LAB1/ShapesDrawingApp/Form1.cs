using ShapesDrawingApp.Collections;
using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp;

public partial class Form1 : Form
{
    private readonly FigureList _figures = new();

    public Form1()
    {
        InitializeComponent();
        Text = "LAB1";
        DoubleBuffered = true;
        InitializeShapes();
    }

    private void InitializeShapes()
    {
        _figures.Add(new LineShape(new Point(30, 30), new Point(220, 80), Color.DarkBlue, 3));
        _figures.Add(new RectangleShape(new Rectangle(260, 30, 180, 100), Color.DarkGreen, 3));
        _figures.Add(new EllipseShape(new Rectangle(480, 30, 180, 100), Color.DarkRed, 3));
        _figures.Add(new CircleShape(new Point(120, 220), 60, Color.Purple, 3));
        _figures.Add(new TriangleShape(
            new Point(300, 260),
            new Point(230, 380),
            new Point(370, 380),
            Color.Orange,
            3));
        _figures.Add(new PolylineShape(
            new[]
            {
                new Point(460, 220),
                new Point(520, 270),
                new Point(500, 340),
                new Point(620, 380),
                new Point(690, 300)
            },
            Color.Brown,
            3));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        _figures.DrawAll(e.Graphics);
    }
}

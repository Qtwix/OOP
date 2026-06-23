using ShapesDrawingApp.Collections;
using ShapesDrawingApp.Factories;
using ShapesDrawingApp.Rendering;
using ShapesDrawingApp.Shapes;

namespace ShapesDrawingApp;

public partial class Form1 : Form
{
    private readonly FigureList _figures = new();
    private readonly ShapeFactoryRegistry _factoryRegistry = new();
    private readonly ShapeRendererService _rendererService = new();

    private IShapeFactory? _selectedFactory;
    private bool _isDragging;
    private Point _dragStart;
    private Rectangle _previewBounds;
    private Color _selectedColor = Color.Black;

    public Form1()
    {
        InitializeComponent();
        Text = "LAB2";
        DoubleBuffered = true;
        InitializeEditor();
    }

    private void InitializeEditor()
    {
        shapeComboBox.DataSource = _factoryRegistry.Factories.ToList();
        shapeComboBox.DisplayMember = nameof(IShapeFactory.DisplayName);
        shapeComboBox.SelectedIndexChanged += shapeComboBox_SelectedIndexChanged;

        if (_factoryRegistry.Factories.Count > 0)
        {
            _selectedFactory = _factoryRegistry.Factories[0];
            shapeComboBox.SelectedIndex = 0;
        }

        colorPreviewPanel.BackColor = _selectedColor;
    }

    private static Rectangle NormalizeBounds(Point first, Point second)
    {
        int left = Math.Min(first.X, second.X);
        int top = Math.Min(first.Y, second.Y);
        int width = Math.Abs(first.X - second.X);
        int height = Math.Abs(first.Y - second.Y);

        return new Rectangle(left, top, width, height);
    }

    private void shapeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _selectedFactory = shapeComboBox.SelectedItem as IShapeFactory;
    }

    private void canvasPanel_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        _isDragging = true;
        _dragStart = e.Location;
        _previewBounds = Rectangle.Empty;
    }

    private void canvasPanel_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }

        _previewBounds = NormalizeBounds(_dragStart, e.Location);
        canvasPanel.Invalidate();
    }

    private void canvasPanel_MouseUp(object sender, MouseEventArgs e)
    {
        if (!_isDragging || _selectedFactory is null)
        {
            return;
        }

        _isDragging = false;
        Rectangle bounds = NormalizeBounds(_dragStart, e.Location);
        if (bounds.Width < 5 || bounds.Height < 5)
        {
            _previewBounds = Rectangle.Empty;
            canvasPanel.Invalidate();
            return;
        }

        Shape shape = _selectedFactory.Create(bounds, _selectedColor, 2);
        _figures.Add(shape);

        _previewBounds = Rectangle.Empty;
        canvasPanel.Invalidate();
    }

    private void canvasPanel_Paint(object sender, PaintEventArgs e)
    {
        foreach (Shape shape in _figures.Items)
        {
            _rendererService.Draw(e.Graphics, shape);
        }

        if (_isDragging && _selectedFactory is not null && _previewBounds.Width > 0 && _previewBounds.Height > 0)
        {
            Shape previewShape = _selectedFactory.Create(_previewBounds, _selectedColor, 1);
            _rendererService.Draw(e.Graphics, previewShape);
        }
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
        _figures.Clear();
        canvasPanel.Invalidate();
    }

    private void colorButton_Click(object sender, EventArgs e)
    {
        using ColorDialog dialog = new()
        {
            AllowFullOpen = true,
            FullOpen = true,
            Color = _selectedColor
        };

        if (dialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        _selectedColor = dialog.Color;
        colorPreviewPanel.BackColor = _selectedColor;
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
}

namespace ShapesDrawingApp;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        topPanel = new Panel();
        colorPreviewPanel = new Panel();
        colorButton = new Button();
        clearButton = new Button();
        shapeLabel = new Label();
        shapeComboBox = new ComboBox();
        canvasPanel = new Panel();
        topPanel.SuspendLayout();
        SuspendLayout();
        // 
        // topPanel
        // 
        topPanel.Controls.Add(colorPreviewPanel);
        topPanel.Controls.Add(colorButton);
        topPanel.Controls.Add(clearButton);
        topPanel.Controls.Add(shapeLabel);
        topPanel.Controls.Add(shapeComboBox);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Size = new Size(1000, 60);
        topPanel.TabIndex = 0;
        // 
        // colorPreviewPanel
        // 
        colorPreviewPanel.BackColor = Color.Black;
        colorPreviewPanel.BorderStyle = BorderStyle.FixedSingle;
        colorPreviewPanel.Location = new Point(560, 15);
        colorPreviewPanel.Name = "colorPreviewPanel";
        colorPreviewPanel.Size = new Size(34, 34);
        colorPreviewPanel.TabIndex = 4;
        // 
        // colorButton
        // 
        colorButton.Location = new Point(450, 14);
        colorButton.Name = "colorButton";
        colorButton.Size = new Size(100, 34);
        colorButton.TabIndex = 3;
        colorButton.Text = "Color";
        colorButton.UseVisualStyleBackColor = true;
        colorButton.Click += colorButton_Click;
        // 
        // clearButton
        // 
        clearButton.Location = new Point(320, 14);
        clearButton.Name = "clearButton";
        clearButton.Size = new Size(112, 34);
        clearButton.TabIndex = 2;
        clearButton.Text = "Clear";
        clearButton.UseVisualStyleBackColor = true;
        clearButton.Click += clearButton_Click;
        // 
        // shapeLabel
        // 
        shapeLabel.AutoSize = true;
        shapeLabel.Location = new Point(20, 19);
        shapeLabel.Name = "shapeLabel";
        shapeLabel.Size = new Size(61, 25);
        shapeLabel.TabIndex = 1;
        shapeLabel.Text = "Shape";
        // 
        // shapeComboBox
        // 
        shapeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        shapeComboBox.FormattingEnabled = true;
        shapeComboBox.Location = new Point(90, 15);
        shapeComboBox.Name = "shapeComboBox";
        shapeComboBox.Size = new Size(210, 33);
        shapeComboBox.TabIndex = 0;
        // 
        // canvasPanel
        // 
        canvasPanel.BackColor = Color.White;
        canvasPanel.Dock = DockStyle.Fill;
        canvasPanel.Location = new Point(0, 60);
        canvasPanel.Name = "canvasPanel";
        canvasPanel.Size = new Size(1000, 540);
        canvasPanel.TabIndex = 1;
        canvasPanel.Paint += canvasPanel_Paint;
        canvasPanel.MouseDown += canvasPanel_MouseDown;
        canvasPanel.MouseMove += canvasPanel_MouseMove;
        canvasPanel.MouseUp += canvasPanel_MouseUp;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 600);
        Controls.Add(canvasPanel);
        Controls.Add(topPanel);
        DoubleBuffered = true;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "LAB2";
        Load += Form1_Load;
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel topPanel;
    private Panel colorPreviewPanel;
    private Button colorButton;
    private Button clearButton;
    private Label shapeLabel;
    private ComboBox shapeComboBox;
    private Panel canvasPanel;
}

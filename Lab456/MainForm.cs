using System.ComponentModel;
using System.Drawing;
using Lab3Serialization.Models;
using Lab3Serialization.Patterns;
using Lab3Serialization.Patterns.Commands;
using Lab3Serialization.Patterns.Observer;
using Lab3Serialization.Plugins;
using Lab3Serialization.Serialization;

namespace Lab3Serialization;

public sealed class MainForm : Form, ICatalogContext
{
    private static readonly Color HeaderBack = Color.FromArgb(247, 248, 250);
    private static readonly Color HeaderBorder = Color.FromArgb(218, 220, 224);
    private static readonly Color AccentBlue = Color.FromArgb(0, 103, 192);

    /// <summary>Высота верхней зоны с учётом строки элементов управления из плагинов.</summary>
    private const int HeaderBarHeight = 322;

    private static FontFamily UiFontFamily =>
        SystemFonts.MessageBoxFont?.FontFamily ?? FontFamily.GenericSansSerif;

    private readonly ListBox _list = new()
    {
        Dock = DockStyle.Fill,
        IntegralHeight = false,
        HorizontalScrollbar = true,
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font(UiFontFamily, 10f),
    };

    private readonly PropertyGrid _grid = new()
    {
        Dock = DockStyle.Fill,
        HelpVisible = true,
        ToolbarVisible = true,
        Font = new Font(UiFontFamily, 9.5f),
        LineColor = Color.FromArgb(240, 240, 244),
        CategoryForeColor = AccentBlue,
    };

    private readonly ComboBox _types = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Font = new Font(UiFontFamily, 10f),
    };

    private readonly SplitContainer _split = new()
    {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Vertical,
        SplitterWidth = 8,
        BackColor = Color.White,
        Panel1MinSize = 1,
        Panel2MinSize = 1,
    };

    private readonly BindingSource _binding = new();

    private readonly PluginLoadReport _pluginReport;
    private readonly FilePipelineService _pipeline;
    private readonly CatalogNotifier _catalogEvents;
    private readonly ToolStripStatusLabel _statusLabel = new()
    {
        Spring = true,
        TextAlign = ContentAlignment.MiddleLeft,
    };

    private readonly FlowLayoutPanel _pluginRibbon = new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
        AutoScroll = false,
        Padding = new Padding(0, 2, 0, 4),
        BackColor = Color.Transparent,
        Margin = new Padding(0),
    };

    public MainForm(PluginLoadReport plugins, FilePipelineService pipeline, CatalogNotifier catalogEvents)
    {
        _pluginReport = plugins;
        _pipeline = pipeline;
        _catalogEvents = catalogEvents;
        _catalogEvents.Subscribe(new StatusStripLogObserver(_statusLabel));

        Text = "Кондитерская";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1420, 840);
        MinimumSize = new Size(1120, 660);
        Padding = new Padding(0);
        Font = new Font(UiFontFamily, 10f);
        BackColor = Color.White;

        foreach (var (kind, title) in GoodsRegistry.KnownTypes)
            _types.Items.Add(new TypePick(kind, title));

        if (_types.Items.Count > 0)
            _types.SelectedIndex = 0;

        var header = BuildHeaderPanel(plugins);

        var grpList = new GroupBox
        {
            Text = " Товары ",
            Dock = DockStyle.Fill,
            Padding = new Padding(12, 10, 12, 12),
            Margin = new Padding(0),
            Font = new Font(Font, FontStyle.Bold),
            ForeColor = Color.FromArgb(55, 55, 65),
        };
        grpList.Controls.Add(_list);

        var grpProps = new GroupBox
        {
            Text = " Свойства ",
            Dock = DockStyle.Fill,
            Padding = new Padding(12, 10, 12, 12),
            Margin = new Padding(0),
            Font = new Font(Font, FontStyle.Bold),
            ForeColor = Color.FromArgb(55, 55, 65),
        };
        grpProps.Controls.Add(_grid);

        var leftShell = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 12, 8, 12), BackColor = Color.White };
        leftShell.Controls.Add(grpList);

        var rightShell = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8, 12, 12, 12), BackColor = Color.White };
        rightShell.Controls.Add(grpProps);

        _split.Panel1.Controls.Add(leftShell);
        _split.Panel2.Controls.Add(rightShell);

        var shell = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.White,
            Padding = new Padding(0),
            Margin = new Padding(0),
        };
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, HeaderBarHeight));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        shell.Controls.Add(header, 0, 0);
        shell.Controls.Add(_split, 0, 1);
        header.Dock = DockStyle.Fill;
        _split.Dock = DockStyle.Fill;

        var menu = new MenuStrip();
        var settingsItem = new ToolStripMenuItem("Настройки");
        var pipelineItem = new ToolStripMenuItem(
            "Обработка при сохранении/загрузке…",
            null,
            (_, _) => OpenPipelineSettings());
        settingsItem.DropDownItems.Add(pipelineItem);

        if (_pipeline.PipelinePlugins.Count > 0)
        {
            settingsItem.DropDownItems.Add(new ToolStripSeparator());
            foreach (var p in _pipeline.PipelinePlugins.OrderBy(x => x.Order))
            {
                var toggle = new ToolStripMenuItem($"{p.Name}")
                {
                    Checked = _pipeline.IsEnabled(p.Id),
                    CheckOnClick = true,
                };
                var id = p.Id;
                toggle.CheckedChanged += (_, _) => _pipeline.SetEnabled(id, toggle.Checked);
                settingsItem.DropDownItems.Add(toggle);
            }
        }

        var helpItem = new ToolStripMenuItem("Справка");
        helpItem.DropDownItems.Add("Паттерны (лаб. 6)…", null, (_, _) =>
        {
            MessageBox.Show(
                this,
                PatternsInfo.HelpText,
                "Паттерны",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        });
        menu.Items.Add(settingsItem);
        menu.Items.Add(helpItem);
        MainMenuStrip = menu;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.White,
            Padding = new Padding(0),
            Margin = new Padding(0),
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.Controls.Add(menu, 0, 0);
        root.Controls.Add(shell, 0, 1);
        Controls.Add(root);

        var statusStrip = new StatusStrip();
        statusStrip.Items.Add(_statusLabel);
        statusStrip.Items.Add(new ToolStripStatusLabel("Лаб. 6 — Adapter / Command / Observer")
        {
            BorderSides = ToolStripStatusLabelBorderSides.Left,
        });
        Controls.Add(statusStrip);

        _catalogEvents.Publish("Приложение запущено.");

        Shown += OnMainFormShown;

        _binding.DataSource = new BindingList<GoodsItem>();
        _list.DataSource = _binding;

        _list.SelectedIndexChanged += (_, _) => SyncGrid();
        _grid.PropertyValueChanged += (_, _) => _binding.ResetBindings(false);
        SyncGrid();

        foreach (var p in plugins.Plugins)
        {
            try
            {
                p.RegisterUi(new PluginUiHostImpl(this));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"Плагин «{p.Name}» не смог добавить элементы интерфейса: {ex.Message}",
                    "Плагины",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }

    private Panel BuildHeaderPanel(PluginLoadReport plugins)
    {
        var header = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24, 16, 24, 14),
            BackColor = HeaderBack,
        };

        header.Paint += (_, e) =>
        {
            using var pen = new Pen(HeaderBorder);
            e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            BackColor = Color.Transparent,
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48f));

        var title = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Каталог кондитерских изделий",
            ForeColor = Color.FromArgb(38, 38, 42),
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font(UiFontFamily, 11f, FontStyle.Bold),
            BackColor = Color.Transparent,
            AutoEllipsis = true,
        };

        var subtitle = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(95, 95, 105),
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font(UiFontFamily, 9f),
            BackColor = Color.Transparent,
            AutoEllipsis = true,
            Text = BuildPluginHintText(plugins),
        };

        if (plugins.Warnings.Count > 0)
        {
            var tip = new ToolTip { InitialDelay = 120, AutomaticDelay = 4000 };
            tip.SetToolTip(subtitle, string.Join(Environment.NewLine, plugins.Warnings));
        }

        var comboRow = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = new Padding(0),
            Padding = new Padding(0),
        };
        comboRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56f));
        comboRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        var lblType = new Label
        {
            Text = "Тип:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.Transparent,
        };

        _types.Dock = DockStyle.Fill;

        comboRow.Controls.Add(lblType, 0, 0);
        comboRow.Controls.Add(_types, 1, 0);

        var rowCrud = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = new Padding(0),
            Padding = new Padding(0),
        };
        rowCrud.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        rowCrud.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

        var btnAdd = CreateHeaderButton(new AddGoodsCommand(this), emphasized: true);
        var btnRemove = CreateHeaderButton(new RemoveGoodsCommand(this));
        rowCrud.Controls.Add(btnAdd, 0, 0);
        rowCrud.Controls.Add(btnRemove, 1, 0);

        var rowFiles = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = new Padding(0),
            Padding = new Padding(0),
        };
        rowFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        rowFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

        var btnSave = CreateHeaderButton(new SaveCatalogCommand(this));
        var btnLoad = CreateHeaderButton(new LoadCatalogCommand(this));
        rowFiles.Controls.Add(btnSave, 0, 0);
        rowFiles.Controls.Add(btnLoad, 1, 0);

        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(subtitle, 0, 1);
        layout.Controls.Add(comboRow, 0, 2);
        layout.Controls.Add(rowCrud, 0, 3);
        layout.Controls.Add(rowFiles, 0, 4);
        layout.Controls.Add(_pluginRibbon, 0, 5);

        header.Controls.Add(layout);
        return header;
    }

    private static string BuildPluginHintText(PluginLoadReport report)
    {
        var dir = string.IsNullOrWhiteSpace(report.PluginsDirectory)
            ? Path.Combine(AppContext.BaseDirectory, "Plugins")
            : report.PluginsDirectory;

        var baseLine =
            $"{report.Plugins.Count} плагин(ов) товаров, {report.FilePipelinePlugins.Count} плагин(ов) файла; папка: {dir}";
        return report.Warnings.Count == 0
            ? baseLine
            : baseLine + ". Есть предупреждения — наведите на эту строку.";
    }

    private Button CreateRibbonButton(string text, Action action)
    {
        var button = new Button
        {
            Text = text,
            Dock = DockStyle.None,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(6, 6, 6, 4),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font(UiFontFamily, 9.5f),
            UseVisualStyleBackColor = false,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(38, 38, 42),
        };

        button.FlatAppearance.BorderColor = Color.FromArgb(200, 203, 212);
        button.Click += (_, _) => action();
        return button;
    }

    private Button CreateHeaderButton(IAppCommand command, bool emphasized = false)
    {
        var button = new Button
        {
            Text = command.Title,
            Dock = DockStyle.Fill,
            Margin = new Padding(10, 6, 10, 6),
            AutoSize = false,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font(UiFontFamily, 10f),
            UseVisualStyleBackColor = false,
        };

        button.FlatAppearance.BorderColor = Color.FromArgb(200, 203, 212);

        if (emphasized)
        {
            button.BackColor = AccentBlue;
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderSize = 0;
        }
        else
        {
            button.BackColor = Color.White;
            button.ForeColor = Color.FromArgb(38, 38, 42);
        }

        button.Click += (_, _) =>
        {
            if (command.CanExecute())
                command.Execute();
            else
                MessageBox.Show(this, "Действие сейчас недоступно.", command.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        return button;
    }

    private void OnMainFormShown(object? sender, EventArgs e)
    {
        Shown -= OnMainFormShown;
        ApplyPreferredSplitterDistance();
        if (_split.ClientSize.Width <= _split.SplitterWidth + 2)
            BeginInvoke(new Action(ApplyPreferredSplitterDistance));
    }

    private void ApplyPreferredSplitterDistance()
    {
        const int preferredDistance = 480;
        const int targetPanel1Min = 340;
        const int targetPanel2Min = 440;

        var width = _split.ClientSize.Width;
        var splitter = _split.SplitterWidth;
        if (width <= splitter + 2)
            return;

        _split.Panel1MinSize = 1;
        _split.Panel2MinSize = 1;

        var p1Min = targetPanel1Min;
        var p2Min = targetPanel2Min;

        while (p1Min + p2Min + splitter > width && p1Min + p2Min > 2)
        {
            if (p1Min >= p2Min && p1Min > 1)
                p1Min--;
            else if (p2Min > 1)
                p2Min--;
        }

        var minDist = p1Min;
        var maxDist = width - p2Min - splitter;
        if (maxDist < minDist)
        {
            p1Min = 1;
            p2Min = 1;
            minDist = 1;
            maxDist = width - splitter - 1;
        }

        var distance = Math.Clamp(preferredDistance, minDist, maxDist);
        _split.SplitterDistance = distance;

        p1Min = Math.Min(p1Min, distance);
        p2Min = Math.Min(p2Min, width - distance - splitter);
        p1Min = Math.Max(1, p1Min);
        p2Min = Math.Max(1, p2Min);

        _split.Panel1MinSize = p1Min;
        _split.Panel2MinSize = p2Min;

        minDist = _split.Panel1MinSize;
        maxDist = width - _split.Panel2MinSize - splitter;
        if (maxDist >= minDist)
        {
            var current = _split.SplitterDistance;
            if (current < minDist || current > maxDist)
                _split.SplitterDistance = Math.Clamp(current, minDist, maxDist);
        }
    }

    private void AddSelected()
    {
        if (_types.SelectedItem is not TypePick pick)
            return;

        var item = GoodsRegistry.CreateNew(pick.Kind);
        AppendGoodsItem(item);
        _catalogEvents.Publish($"Добавлен товар ({pick.Title}).");
    }

    private void RemoveSelected()
    {
        if (_list.SelectedItem is not GoodsItem item)
            return;

        var idx = _list.SelectedIndex;
        _binding.Remove(item);
        if (_binding.Count > 0)
            _list.SelectedIndex = Math.Min(idx, _binding.Count - 1);
        SyncGrid();
        _catalogEvents.Publish($"Удалён товар: {item}.");
    }

    private void SyncGrid()
    {
        _grid.SelectedObject = _list.SelectedItem;
    }

    private void AppendGoodsItem(GoodsItem item)
    {
        _binding.Add(item);
        _list.SelectedIndex = _list.Items.Count - 1;
        SyncGrid();
    }

    private void OpenPipelineSettings()
    {
        using var dlg = new PipelineSettingsForm(_pipeline, _pluginReport, RefreshTypeCombo);
        if (dlg.ShowDialog(this) == DialogResult.OK)
            RebuildSettingsMenu();
    }

    private void RebuildSettingsMenu()
    {
        if (MainMenuStrip?.Items.Count != 1 ||
            MainMenuStrip.Items[0] is not ToolStripMenuItem settingsItem)
            return;

        while (settingsItem.DropDownItems.Count > 1)
            settingsItem.DropDownItems.RemoveAt(settingsItem.DropDownItems.Count - 1);

        if (_pipeline.PipelinePlugins.Count == 0)
            return;

        settingsItem.DropDownItems.Add(new ToolStripSeparator());
        foreach (var p in _pipeline.PipelinePlugins.OrderBy(x => x.Order))
        {
            var toggle = new ToolStripMenuItem(p.Name)
            {
                Checked = _pipeline.IsEnabled(p.Id),
                CheckOnClick = true,
            };
            var id = p.Id;
            toggle.CheckedChanged += (_, _) => _pipeline.SetEnabled(id, toggle.Checked);
            settingsItem.DropDownItems.Add(toggle);
        }
    }

    private void RefreshTypeCombo()
    {
        var previousKind = (_types.SelectedItem as TypePick)?.Kind;
        _types.Items.Clear();
        foreach (var (kind, title) in GoodsRegistry.KnownTypes)
            _types.Items.Add(new TypePick(kind, title));

        if (previousKind is not null)
        {
            for (var i = 0; i < _types.Items.Count; i++)
            {
                if (_types.Items[i] is TypePick pick && pick.Kind == previousKind)
                {
                    _types.SelectedIndex = i;
                    return;
                }
            }
        }

        if (_types.Items.Count > 0 && _types.SelectedIndex < 0)
            _types.SelectedIndex = 0;
    }

    private void SaveBson()
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "BSON (*.bson)|*.bson|Все файлы|*.*",
            DefaultExt = "bson",
            FileName = "goods.bson",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            var items = EnumerateItems().ToList();
            using var ms = new MemoryStream();
            GoodsListBsonCodec.Serialize(items, ms);
            var processed = _pipeline.ProcessForSave(ms.ToArray());
            File.WriteAllBytes(dlg.FileName, processed);

            var hint = _pipeline.EnabledPluginsInOrder.Count == 0
                ? "Файл сохранён без обработки плагинами."
                : $"Сохранено с плагинами: {string.Join(", ", _pipeline.EnabledPluginsInOrder.Select(p => p.Name))}.";
            MessageBox.Show(this, hint, "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _catalogEvents.Publish($"Сохранено {items.Count} товар(ов) → {dlg.FileName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _catalogEvents.Publish($"Ошибка сохранения: {ex.Message}");
        }
    }

    private void LoadBson()
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "BSON (*.bson)|*.bson|Все файлы|*.*",
            DefaultExt = "bson",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        List<GoodsItem> loaded;
        try
        {
            var raw = File.ReadAllBytes(dlg.FileName);
            var payload = _pipeline.ProcessAfterLoad(raw);
            using var ms = new MemoryStream(payload);
            loaded = GoodsListBsonCodec.Deserialize(ms);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message, "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _catalogEvents.Publish($"Ошибка загрузки: {ex.Message}");
            return;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _catalogEvents.Publish($"Ошибка загрузки: {ex.Message}");
            return;
        }

        _binding.Clear();
        foreach (var item in loaded)
            _binding.Add(item);

        if (_binding.Count > 0)
            _list.SelectedIndex = 0;
        SyncGrid();
        _catalogEvents.Publish($"Загружено {loaded.Count} товар(ов) из {dlg.FileName}");
    }

    private IEnumerable<GoodsItem> EnumerateItems()
    {
        foreach (GoodsItem item in _binding.List)
            yield return item;
    }

    private sealed record TypePick(string Kind, string Title)
    {
        public override string ToString() => Title;
    }

    bool ICatalogContext.HasSelection => _list.SelectedItem is GoodsItem;

    bool ICatalogContext.HasTypeSelected => _types.SelectedItem is TypePick;

    void ICatalogContext.AddFromSelection() => AddSelected();

    void ICatalogContext.RemoveSelected() => RemoveSelected();

    void ICatalogContext.SaveToFile() => SaveBson();

    void ICatalogContext.LoadFromFile() => LoadBson();

    private sealed class PluginUiHostImpl : IPluginUiHost
    {
        private readonly MainForm _owner;

        public PluginUiHostImpl(MainForm owner) => _owner = owner;

        public Form Shell => _owner;

        public void AddToolbarButton(string text, Action click)
        {
            var button = _owner.CreateRibbonButton(text, click);
            _owner._pluginRibbon.Controls.Add(button);
        }

        public IEnumerable<GoodsItem> CurrentItemsSnapshot()
        {
            foreach (GoodsItem item in _owner._binding.List)
                yield return item;
        }

        public IEnumerable<GoodsItem> SelectedItems()
        {
            foreach (var i in _owner._list.SelectedItems)
            {
                if (i is GoodsItem g)
                    yield return g;
            }
        }

        public void RefreshItemViews() => _owner._binding.ResetBindings(false);

        public void AppendNewItem(GoodsItem item) => _owner.AppendGoodsItem(item);
    }
}

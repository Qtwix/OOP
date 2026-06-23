using Lab3Serialization.Plugins;
using Lab3Serialization.Serialization;

namespace Lab3Serialization;

public sealed class PipelineSettingsForm : Form
{
    private readonly FilePipelineService _pipeline;
    private readonly PluginLoadReport _report;
    private readonly CheckedListBox _list;
    private readonly Action? _onGoodsPluginsChanged;
    private List<IFilePipelinePlugin> _ordered = new();

    public PipelineSettingsForm(
        FilePipelineService pipeline,
        PluginLoadReport report,
        Action? onGoodsPluginsChanged = null)
    {
        _pipeline = pipeline;
        _report = report;
        _onGoodsPluginsChanged = onGoodsPluginsChanged;

        Text = "Настройки — обработка файлов";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(560, 420);
        Font = SystemFonts.MessageBoxFont;

        var intro = new Label
        {
            Dock = DockStyle.Top,
            Height = 72,
            Padding = new Padding(12, 12, 12, 4),
            Text =
                "Ваши плагины лаб. 5: CRC32 (вариант 5), SHA-256, заголовок.\n" +
                "DLL друга: Lab5CryptoPlugin.dll (вариант 3). Ваш адаптер: CryptoPluginAdapter.dll.\n" +
                "Файлы — в папке Plugins, догрузка — кнопкой ниже.",
        };

        _list = new CheckedListBox
        {
            Dock = DockStyle.Fill,
            CheckOnClick = true,
            IntegralHeight = false,
            BorderStyle = BorderStyle.FixedSingle,
        };

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(8, 6, 8, 8),
            WrapContents = false,
        };

        var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
        var btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, AutoSize = true };
        var btnLoadDll = new Button { Text = "Загрузить плагин…", AutoSize = true };
        btnLoadDll.Click += OnLoadDllClick;

        buttons.Controls.Add(btnOk);
        buttons.Controls.Add(btnCancel);
        buttons.Controls.Add(btnLoadDll);

        var center = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 12, 0) };
        center.Controls.Add(_list);

        Controls.Add(center);
        Controls.Add(buttons);
        Controls.Add(intro);

        AcceptButton = btnOk;
        CancelButton = btnCancel;

        RefreshList();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (DialogResult == DialogResult.OK)
            ApplyChecks();
        base.OnFormClosing(e);
    }

    private void RefreshList()
    {
        _ordered = _pipeline.PipelinePlugins
            .OrderBy(p => p.Order)
            .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        _list.Items.Clear();
        foreach (var plugin in _ordered)
        {
            var idx = _list.Items.Add($"{plugin.Name}  [{plugin.Id}] — {plugin.Description}");
            _list.SetItemChecked(idx, _pipeline.IsEnabled(plugin.Id));
        }

        if (_list.Items.Count == 0)
            _list.Items.Add("(нет плагинов обработки — положите DLL в папку Plugins)");
    }

    private void ApplyChecks()
    {
        for (var i = 0; i < _ordered.Count; i++)
            _pipeline.SetEnabled(_ordered[i].Id, _list.GetItemChecked(i));
    }

    private void OnLoadDllClick(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "Плагины (*.dll)|*.dll|Все файлы|*.*",
            Title = "Выберите DLL плагина",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        PluginLoader.LoadFromDll(dlg.FileName, _report);

        foreach (var plugin in _report.FilePipelinePlugins)
        {
            if (_pipeline.PipelinePlugins.Any(p => string.Equals(p.Id, plugin.Id, StringComparison.OrdinalIgnoreCase)))
                continue;
            _pipeline.RegisterRuntimePlugin(plugin);
        }

        _onGoodsPluginsChanged?.Invoke();
        RefreshList();
    }
}

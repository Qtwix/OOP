using System.Windows.Forms;
using Lab3Serialization.Models;
using Lab3Serialization.Plugins;
using Lab3Serialization.Patterns.Observer;
using Lab3Serialization.Serialization;

namespace Lab3Serialization;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
 
        HostAssemblyResolver.Attach();

        ApplicationConfiguration.Initialize();

        // Регистрируем встроенные типы (Candy/Drink/...) в GoodsRegistry.
        GoodsBootstrap.WarmUp();

        GoodsPluginCliResult cli;
        try
        {
            // Опционально: --plugin путь.dll и/или --plugins-dir папка
            cli = GoodsPluginCli.Parse(args);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Параметры плагинов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cli = new GoodsPluginCliResult(Array.Empty<string>(), null);
        }

        // Ключевая строка: загрузка DLL плагинов, создание IGoodsPlugin и вызов RegisterTypes().
        var report = PluginLoader.Load(cli);
        var pipeline = new FilePipelineService(report.FilePipelinePlugins);
        var catalogEvents = new CatalogNotifier();
        Application.Run(new MainForm(report, pipeline, catalogEvents));
    }
}

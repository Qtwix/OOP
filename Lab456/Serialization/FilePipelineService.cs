using Lab3Serialization.Plugins;

namespace Lab3Serialization.Serialization;

public sealed class FilePipelineService
{
    private readonly Dictionary<string, bool> _enabled = new(StringComparer.OrdinalIgnoreCase);

    public FilePipelineService(IEnumerable<IFilePipelinePlugin> plugins)
    {
        foreach (var p in plugins)
        {
            PipelinePlugins.Add(p);
            _enabled[p.Id] = true;
        }
    }

    public List<IFilePipelinePlugin> PipelinePlugins { get; } = new();

    public IReadOnlyList<IFilePipelinePlugin> EnabledPluginsInOrder =>
        PipelinePlugins
            .Where(p => IsEnabled(p.Id))
            .OrderBy(p => p.Order)
            .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

    public bool IsEnabled(string id) =>
        _enabled.TryGetValue(id, out var on) && on;

    public void SetEnabled(string id, bool enabled) => _enabled[id] = enabled;

    public void RegisterRuntimePlugin(IFilePipelinePlugin plugin, bool enabled = true)
    {
        if (PipelinePlugins.Any(p => string.Equals(p.Id, plugin.Id, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Плагин с Id «{plugin.Id}» уже загружен.");

        PipelinePlugins.Add(plugin);
        _enabled[plugin.Id] = enabled;
    }

    public byte[] ProcessForSave(byte[] bsonPayload)
    {
        var data = bsonPayload;
        foreach (var plugin in EnabledPluginsInOrder)
            data = plugin.TransformBeforeSave(data);
        return data;
    }

    public byte[] ProcessAfterLoad(byte[] fileBytes)
    {
        var data = fileBytes;
        foreach (var plugin in EnabledPluginsInOrder.Reverse())
            data = plugin.TransformAfterLoad(data);
        return data;
    }
}

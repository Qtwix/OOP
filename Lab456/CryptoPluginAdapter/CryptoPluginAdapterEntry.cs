using Lab3Serialization.Plugins;
using Lab5CryptoPlugin;

namespace CryptoPluginAdapter;

public sealed class CryptoPluginAdapterEntry : IFilePipelinePlugin
{
    private readonly CryptoServiceToPipelineAdapter _inner = new(new AesCryptoFileService());

    public string Id => _inner.Id;

    public string Name => _inner.Name;

    public string Description => _inner.Description;

    public int Order => _inner.Order;

    public byte[] TransformBeforeSave(byte[] payload) => _inner.TransformBeforeSave(payload);

    public byte[] TransformAfterLoad(byte[] stored) => _inner.TransformAfterLoad(stored);
}

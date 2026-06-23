using Lab3Serialization.Plugins;
using Lab5CryptoPlugin;

namespace CryptoPluginAdapter;

internal sealed class CryptoServiceToPipelineAdapter : IFilePipelinePlugin
{
    private readonly ICryptoFileService _crypto;

    public CryptoServiceToPipelineAdapter(ICryptoFileService crypto) => _crypto = crypto;

    public string Id => "lab5crypto-adapter";

    public string Name => $"Адаптер → {_crypto.ServiceName} (Lab5CryptoPlugin.dll)";

    public string Description =>
        "Паттерн Adapter: ICryptoFileService → IFilePipelinePlugin хоста.";

    public int Order => 150;

    public byte[] TransformBeforeSave(byte[] payload) => _crypto.EncryptBytes(payload);

    public byte[] TransformAfterLoad(byte[] stored) => _crypto.Decrypt Bytes(stored);
}

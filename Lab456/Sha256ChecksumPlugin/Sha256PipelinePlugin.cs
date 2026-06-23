using System.Security.Cryptography;
using Lab3Serialization.Plugins;

namespace Sha256ChecksumPlugin;

/// <summary>добавляет 32 байта sha-256 в конец файла</summary>
public sealed class Sha256PipelinePlugin : IFilePipelinePlugin
{
    private const int HashLength = 32; // размер хеша в байтах

    public string Id => "sha256"; // идентификатор

    public string Name => "SHA-256 контрольная сумма"; // имя плагина

    public string Description => "добавляет 32 байта хеша sha-256; при загрузке сверяет данные."; // описание

    public int Order => 200; // порядок (между crc32 и заголовком)

    public byte[] TransformBeforeSave(byte[] payload)
    {
        // вычисляем хеш от данных
        var hash = SHA256.HashData(payload);
        // создаём массив с местом для хеша
        var result = new byte[payload.Length + HashLength];
        // копируем данные в начало
        Buffer.BlockCopy(payload, 0, result, 0, payload.Length);
        // добавляем хеш в конец
        Buffer.BlockCopy(hash, 0, result, payload.Length, HashLength);
        return result;
    }

    public byte[] TransformAfterLoad(byte[] stored)
    {
        // проверяем, что хватит места для хеша
        if (stored.Length < HashLength)
            throw new InvalidOperationException("файл слишком короткий для sha-256.");

        // отделяем данные от хеша
        var payloadLength = stored.Length - HashLength;
        var payload = stored.AsSpan(0, payloadLength);
        var expected = stored.AsSpan(payloadLength, HashLength);
        // пересчитываем хеш
        var actual = SHA256.HashData(payload);

        // сравниваем хеши
        if (!expected.SequenceEqual(actual))
            throw new InvalidOperationException("контрольная сумма sha-256 не совпадает.");

        // возвращаем данные без хеша
        return stored[..payloadLength];
    }
}
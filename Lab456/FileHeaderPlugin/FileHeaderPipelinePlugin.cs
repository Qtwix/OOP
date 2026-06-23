using System.Buffers.Binary;
using Lab3Serialization.Plugins;

namespace FileHeaderPlugin;

/// <summary>добавляет текстовый заголовок с длиной bson части</summary>
public sealed class FileHeaderPipelinePlugin : IFilePipelinePlugin
{
    // магическая метка + нуль-терминатор
    private static readonly byte[] Magic = "LAB5HDR\0"u8.ToArray();

    public string Id => "lab5-header"; // идентификатор плагина

    public string Name => "Заголовок LAB5"; // человеко-читаемое имя

    public string Description => "префикс с меткой lab5hdr и длиной данных."; // описание

    public int Order => 300; // порядок выполнения (после crc32)

    public byte[] TransformBeforeSave(byte[] payload)
    {
        // создаём заголовок: магия + 4 байта длины
        var header = new byte[Magic.Length + 4];
        Buffer.BlockCopy(Magic, 0, header, 0, Magic.Length);
        // пишем длину полезной нагрузки (little-endian)
        BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(Magic.Length), payload.Length);

        // склеиваем заголовок и данные
        var result = new byte[header.Length + payload.Length];
        Buffer.BlockCopy(header, 0, result, 0, header.Length);
        Buffer.BlockCopy(payload, 0, result, header.Length, payload.Length);
        return result;
    }

    public byte[] TransformAfterLoad(byte[] stored)
    {
        var min = Magic.Length + 4; // минимальный размер: магия + длина
        if (stored.Length < min)
            throw new InvalidOperationException("файл слишком короткий для заголовка lab5hdr.");

        // проверяем магическую метку
        if (!stored.AsSpan(0, Magic.Length).SequenceEqual(Magic))
            throw new InvalidOperationException("отсутствует заголовок lab5hdr — файл сохранён без этого плагина?");

        // читаем объявленную длину данных
        var length = BinaryPrimitives.ReadInt32LittleEndian(stored.AsSpan(Magic.Length, 4));
        var expectedTotal = min + length;

        // сверяем реальный размер файла с ожидаемым
        if (stored.Length != expectedTotal)
        {
            throw new InvalidOperationException(
                $"неверная длина в заголовке: объявлено {length} байт данных, в файле {stored.Length - min}.");
        }

        // возвращаем только полезные данные (без заголовка)
        return stored[min..];
    }
}
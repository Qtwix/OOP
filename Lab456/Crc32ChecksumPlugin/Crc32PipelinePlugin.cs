using System.Buffers.Binary;
using Lab3Serialization.Plugins;

namespace Crc32ChecksumPlugin;

/// <summary>к концу файла добавляется crc32 (4 байта, little-endian).</summary>
public sealed class Crc32PipelinePlugin : IFilePipelinePlugin
{
    public string Id => "crc32"; // идентификатор

    public string Name => "CRC32 контрольная сумма"; // имя плагина

    public string Description => "добавляет 4 байта crc32 ieee в конец файла; при загрузке проверяет целостность."; // описание

    public int Order => 100; // порядок выполнения

    public byte[] TransformBeforeSave(byte[] payload)
    {
        // считаем crc от данных
        var crc = Crc32.Compute(payload);
        // создаём массив на 4 байта больше
        var result = new byte[payload.Length + 4];
        // копируем исходные данные
        Buffer.BlockCopy(payload, 0, result, 0, payload.Length);
        // записываем crc в конец (little-endian)
        BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(payload.Length), crc);
        return result;
    }

    public byte[] TransformAfterLoad(byte[] stored)
    {
        // проверяем, что есть место для crc
        if (stored.Length < 4)
            throw new InvalidOperationException("файл слишком короткий для crc32 (ожидается минимум 4 байта суммы).");

        // отделяем данные от контрольной суммы
        var payloadLength = stored.Length - 4;
        var payload = stored.AsSpan(0, payloadLength);
        // читаем ожидаемую crc из файла
        var expected = BinaryPrimitives.ReadUInt32LittleEndian(stored.AsSpan(payloadLength));
        // вычисляем актуальную crc
        var actual = Crc32.Compute(payload);

        // сверяем суммы
        if (expected != actual)
        {
            throw new InvalidOperationException(
                $"контрольная сумма crc32 не совпадает (в файле: 0x{expected:X8}, вычислено: 0x{actual:X8}).");
        }

        // возвращаем данные без crc
        return stored[..payloadLength];
    }
}

internal static class Crc32
{
    private static readonly uint[] Table = CreateTable(); // таблица для быстрого расчёта

    public static uint Compute(ReadOnlySpan<byte> data)
    {
        uint crc = 0xFFFFFFFF; // начальное значение
        foreach (var b in data)
            crc = Table[(crc ^ b) & 0xFF] ^ (crc >> 8); // основной цикл
        return crc ^ 0xFFFFFFFF; // финальный xor
    }

    private static uint[] CreateTable()
    {
        const uint polynomial = 0xEDB88320; // полином crc32 ieee
        var table = new uint[256];
        for (uint i = 0; i < 256; i++)
        {
            var crc = i;
            for (var j = 0; j < 8; j++)
                crc = (crc & 1) != 0 ? polynomial ^ (crc >> 1) : crc >> 1; // битовый сдвиг
            table[i] = crc;
        }

        return table;
    }
}
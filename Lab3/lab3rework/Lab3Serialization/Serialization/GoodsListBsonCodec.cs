using Lab3Serialization.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace Lab3Serialization.Serialization;

public static class GoodsListBsonCodec
{
    public static void Serialize(IReadOnlyList<GoodsItem> items, Stream stream)
    {
        var array = new BsonArray();
        foreach (var item in items)
            array.Add(item.ToBsonDocument());

        var root = new BsonDocument { ["items"] = array };

        using var writer = new BsonBinaryWriter(stream);
        BsonSerializer.Serialize(writer, root);
    }

    public static List<GoodsItem> Deserialize(Stream stream)
    {
        using var reader = new BsonBinaryReader(stream);
        var root = BsonSerializer.Deserialize<BsonDocument>(reader);
        var array = root["items"].AsBsonArray;
        var list = new List<GoodsItem>();
        foreach (var element in array)
        {
            var doc = element.AsBsonDocument;
            list.Add(GoodsRegistry.FromBsonDocument(doc));
        }

        return list;
    }
}

using MongoDB.Bson;

namespace Lab3Serialization.Models;


public static class GoodsRegistry
{
    private static readonly Dictionary<string, Func<BsonDocument, GoodsItem>> FromDocument = new();
    private static readonly Dictionary<string, Func<GoodsItem>> NewItemFactories = new();
    private static readonly List<(string Kind, string Title)> OrderedTypes = new();

    public static void Register(string kind, string title, Func<GoodsItem> createNew, Func<BsonDocument, GoodsItem> fromDocument)
    {
        OrderedTypes.Add((kind, title));
        NewItemFactories[kind] = createNew;
        FromDocument[kind] = fromDocument;
    }

    public static IReadOnlyList<(string Kind, string Title)> KnownTypes => OrderedTypes;

    public static GoodsItem CreateNew(string kind) => NewItemFactories[kind]();

    public static GoodsItem FromBsonDocument(BsonDocument doc)
    {
        var kind = doc["kind"].AsString;
        var factory = FromDocument[kind];
        return factory(doc);
    }
}

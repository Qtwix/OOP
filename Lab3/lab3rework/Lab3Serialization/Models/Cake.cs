using MongoDB.Bson;

namespace Lab3Serialization.Models;

public sealed class Cake : GoodsItem
{
    static Cake()
    {
        GoodsRegistry.Register(
            KindConst,
            "Торт",
            () => new Cake(),
            static doc =>
            {
                var item = new Cake();
                ReadBase(doc, item);
                item.SliceCount = doc["sliceCount"].AsInt32;
                return item;
            });
    }

    private const string KindConst = "Cake";

    public int SliceCount { get; set; }

    public override string Kind => KindConst;

    protected override void AppendFields(BsonDocument doc) => doc["sliceCount"] = SliceCount;
}

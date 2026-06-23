using MongoDB.Bson;

namespace Lab3Serialization.Models;

public sealed class ChocolateBar : GoodsItem
{
    static ChocolateBar()
    {
        GoodsRegistry.Register(
            KindConst,
            "Плитка шоколада",
            () => new ChocolateBar(),
            static doc =>
            {
                var item = new ChocolateBar();
                ReadBase(doc, item);
                item.CocoaPercent = doc["cocoaPercent"].AsInt32;
                return item;
            });
    }

    private const string KindConst = "ChocolateBar";

    public int CocoaPercent { get; set; }

    public override string Kind => KindConst;

    protected override void AppendFields(BsonDocument doc) => doc["cocoaPercent"] = CocoaPercent;
}

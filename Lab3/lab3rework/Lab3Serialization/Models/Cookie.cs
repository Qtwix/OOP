using MongoDB.Bson;

namespace Lab3Serialization.Models;

public sealed class Cookie : GoodsItem
{
    static Cookie()
    {
        GoodsRegistry.Register(
            KindConst,
            "Печенье",
            () => new Cookie(),
            static doc =>
            {
                var item = new Cookie();
                ReadBase(doc, item);
                item.WeightGrams = doc["weightGrams"].AsInt32;
                return item;
            });
    }

    private const string KindConst = "Cookie";

    public int WeightGrams { get; set; }

    public override string Kind => KindConst;

    protected override void AppendFields(BsonDocument doc) => doc["weightGrams"] = WeightGrams;
}

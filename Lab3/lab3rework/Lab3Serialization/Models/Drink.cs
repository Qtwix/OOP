using MongoDB.Bson;

namespace Lab3Serialization.Models;

public sealed class Drink : GoodsItem
{
    static Drink()
    {
        GoodsRegistry.Register(
            KindConst,
            "Напиток",
            () => new Drink(),
            static doc =>
            {
                var item = new Drink();
                ReadBase(doc, item);
                item.VolumeMl = doc["volumeMl"].AsInt32;
                return item;
            });
    }

    private const string KindConst = "Drink";

    public int VolumeMl { get; set; }

    public override string Kind => KindConst;

    protected override void AppendFields(BsonDocument doc) => doc["volumeMl"] = VolumeMl;
}

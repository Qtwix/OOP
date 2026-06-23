using MongoDB.Bson;

namespace Lab3Serialization.Models;

public sealed class Marmalade : GoodsItem
{
    static Marmalade()
    {
        GoodsRegistry.Register(
            KindConst,
            "Мармелад",
            () => new Marmalade(),
            static doc =>
            {
                var item = new Marmalade();
                ReadBase(doc, item);
                item.Fruit = doc["fruit"].AsString;
                return item;
            });
    }

    private const string KindConst = "Marmalade";

    public string Fruit { get; set; } = "";

    public override string Kind => KindConst;

    protected override void AppendFields(BsonDocument doc) => doc["fruit"] = Fruit;
}

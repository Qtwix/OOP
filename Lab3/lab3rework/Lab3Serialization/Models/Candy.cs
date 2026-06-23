using MongoDB.Bson;

namespace Lab3Serialization.Models;

public sealed class Candy : GoodsItem
{
    static Candy()
    {
        GoodsRegistry.Register(
            KindConst,
            "Конфета",
            () => new Candy(),
            static doc =>
            {
                var item = new Candy();
                ReadBase(doc, item);
                item.Flavor = doc["flavor"].AsString;
                return item;
            });
    }

    private const string KindConst = "Candy";

    public string Flavor { get; set; } = "";

    public override string Kind => KindConst;

    protected override void AppendFields(BsonDocument doc) => doc["flavor"] = Flavor;
}

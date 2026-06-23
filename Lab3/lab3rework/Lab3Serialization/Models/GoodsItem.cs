using MongoDB.Bson;

namespace Lab3Serialization.Models;

public abstract class GoodsItem
{
    public string Name { get; set; } = "";

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public abstract string Kind { get; }

    public BsonDocument ToBsonDocument()
    {
        var doc = new BsonDocument
        {
            ["kind"] = Kind,
            ["name"] = Name,
            ["price"] = (double)Price,
            ["stock"] = Stock,
        };
        AppendFields(doc);
        return doc;
    }

    protected abstract void AppendFields(BsonDocument doc);

    protected static void ReadBase(BsonDocument doc, GoodsItem target)
    {
        target.Name = doc["name"].AsString;
        target.Price = Convert.ToDecimal(doc["price"].AsDouble);
        target.Stock = doc["stock"].AsInt32;
    }

    public override string ToString() => $"{Kind}: {Name}, {Price} ₽, остаток {Stock}";
}

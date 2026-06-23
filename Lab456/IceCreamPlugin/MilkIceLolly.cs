using Lab3Serialization.Models;
using MongoDB.Bson;

namespace IceCreamPlugin;

/// <summary>Новый вид товара из плагина — наследует базовый <see cref="GoodsItem"/>.</summary>
public sealed class MilkIceLolly : GoodsItem
{
    public const string KindConst = "MilkIceLolly";

    public string GlazeFlavor { get; set; } = "";

    public bool OnStick { get; set; } = true;

    public override string Kind => KindConst;

    public static MilkIceLolly FromBson(BsonDocument doc)
    {
        var item = new MilkIceLolly();
        ReadBase(doc, item);
        item.GlazeFlavor = doc["glazeFlavor"].AsString;
        item.OnStick = doc["onStick"].AsBoolean;
        return item;
    }

    protected override void AppendFields(BsonDocument doc)
    {
        doc["glazeFlavor"] = GlazeFlavor;
        doc["onStick"] = OnStick;
    }
}

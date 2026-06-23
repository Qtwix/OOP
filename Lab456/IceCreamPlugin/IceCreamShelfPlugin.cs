using Lab3Serialization.Models;
using Lab3Serialization.Plugins;

namespace IceCreamPlugin;

/// <summary>Точка входа плагина: регистрирует новый класс и добавляет кнопки в главное окно.</summary>
public sealed class IceCreamShelfPlugin : IGoodsPlugin
{
    public string Name => "Мороженое (IceCreamPlugin)";

    public void RegisterTypes()
    {
        GoodsRegistry.Register(
            MilkIceLolly.KindConst,
            "Эскимо (плагин)",
            static () => new MilkIceLolly(),
            MilkIceLolly.FromBson);
    }

    public void RegisterUi(IPluginUiHost host)
    {
        host.AddToolbarButton("Добавить демо-эскимо", () =>
        {
            var demo = new MilkIceLolly
            {
                Name = "Белочка",
                Price = 120,
                Stock = 18,
                GlazeFlavor = "молочный шоколад",
                OnStick = true,
            };

            host.AppendNewItem(demo);
        });

        host.AddToolbarButton("Скидка 15% выбранному эскимо", () =>
        {
            foreach (var item in host.SelectedItems())
            {
                if (item is not MilkIceLolly ice)
                    continue;

                ice.Price = Math.Round(ice.Price * 0.85m, 2, MidpointRounding.AwayFromZero);
            }

            host.RefreshItemViews();
        });
    }
}

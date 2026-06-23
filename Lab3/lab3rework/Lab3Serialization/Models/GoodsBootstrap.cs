using System.Runtime.CompilerServices;

namespace Lab3Serialization.Models;

// Принудительно вызывает статические конструкторы всех классов товаров.
// Это нужно, чтобы каждый класс записал свои фабрики в GoodsRegistry
public static class GoodsBootstrap
{
    public static void WarmUp()
    {
        RuntimeHelpers.RunClassConstructor(typeof(Candy).TypeHandle);
        RuntimeHelpers.RunClassConstructor(typeof(Cookie).TypeHandle);
        RuntimeHelpers.RunClassConstructor(typeof(Drink).TypeHandle);
        RuntimeHelpers.RunClassConstructor(typeof(Cake).TypeHandle);
        RuntimeHelpers.RunClassConstructor(typeof(ChocolateBar).TypeHandle);
        RuntimeHelpers.RunClassConstructor(typeof(Marmalade).TypeHandle);
    }
}

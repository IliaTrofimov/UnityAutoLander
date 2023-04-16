using System;

namespace Lander.Shared
{
    /// <summary>Объект с произвольной пользовательской меткой. Предназначен для визуального опознавания объектов.</summary>
    /// <typeparam name="T">Тип метки.</typeparam>
    public interface ILabeledWithOrder<T> 
    {
        public (T label, int order) GetLabel();
    }

    /// <summary>Объект с произвольной пользовательской меткой. Предназначен для визуального опознавания объектов.</summary>
    /// <typeparam name="T">Тип метки.</typeparam>
    public interface ILabeled<T>
    {
        public T GetLabel();
    }
}
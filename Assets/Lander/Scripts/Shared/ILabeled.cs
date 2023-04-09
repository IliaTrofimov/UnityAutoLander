using System;

namespace Lander.Shared
{
    /// <summary>Объект с произвольной пользовательской меткой. Предназначен для визуального опознавания объектов.</summary>
    /// <typeparam name="T">Тип метки.</typeparam>
    public interface ILabeledWithOrder<T> where T: struct 
    {
        public (T label, int order) GetLabel();
    }

    /// <summary>Объект с произвольной пользовательской меткой. Предназначен для визуального опознавания объектов.</summary>
    /// <typeparam name="T">Тип метки.</typeparam>
    public interface ILabeled<T> where T : struct
    {
        public T GetLabel();
    }
}
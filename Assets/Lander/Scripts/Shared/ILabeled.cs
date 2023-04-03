using System;

namespace Shared
{
    /// <summary>Объект с произвольной пользовательской меткой. Предназначен для визуального опознавания объектов.</summary>
    /// <typeparam name="T">Тип метки.</typeparam>
    public interface ILabeledWithOrder<T> : ILabeled<(T label, int order)> where T: struct 
    {
        
    }

    /// <summary>Объект с произвольной пользовательской меткой. Предназначен для визуального опознавания объектов.</summary>
    /// <typeparam name="T">Тип метки.</typeparam>
    public interface ILabeled<T> where T : struct
    {
        public T GetLabel();
    }
}
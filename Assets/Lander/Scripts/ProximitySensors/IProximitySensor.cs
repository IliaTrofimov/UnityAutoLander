using UnityEngine.Events;

namespace ProximitySensors
{
	/// <summary>Функционал произвольного сенсора расстояния.</summary>
    public interface IProximitySensor
	{
		/// <summary>Вычисление расстояния до ближайшего объекта по заданной оси.</summary>
		/// <returns>Значение расстояния, если сенсор смог обнаружить объект или <see cref="float.PositiveInfinity"/>, если не смог ничего обнаружить.</returns>
		public float GetDistance();

		/// <summary>Значение последнего измеренного расстояния.</summary>
		public float Distance { get; }
    }
}
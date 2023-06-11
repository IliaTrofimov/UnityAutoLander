using UnityEngine;
using Lander.Shared;
using Unity.Collections;

namespace Lander.Thrusters
{
    /// <summary>Базовый класс для двигателей.</summary>
    public abstract class BaseThruster : MonoBehaviour
    {
        /// <summary>Максимальное значение тяги, <c>MaxThrustValue ≥ 0</c>.</summary>
        public float MaxThrustValue
        {
            get => maxThrustValue;
            set
            {
                if (value > 1000 || value < 0.001f)
                    throw new System.ArgumentException($"{nameof(MaxThrustValue)} must be greater than 1 but value={value}.");
                maxThrustValue = value;
            }
        }

        /// <summary>Включить двигатель с тягой <c>thrust ≥ 0</c>.</summary>
        /// <param name="thrust">Текущее значение тяги, <c>thrust ≥ 0</c>.</param>
        public abstract void Burn(float thrust);

        /// <summary>Выключить двигатель.</summary>
        public abstract void Shutdown();


        [SerializeField]
        [Range(0.001f, 1000f)]
        protected float maxThrustValue;

        [Readonly]
        [SerializeField]
        protected float thrust;
    }
}
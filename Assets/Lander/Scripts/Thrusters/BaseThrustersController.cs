using Lander.Shared;
using Unity.Collections;
using UnityEngine;

namespace Lander.Thrusters
{
    /// <summary>Функционал для согласованного управления системой из нескольких двигателей.</summary>
    public abstract class BaseThrustersController : MonoBehaviour
    {
        [Header("Fuel")]
        [SerializeField]
        [Range(1f, 1000000f)]
        protected float maxFuel;

        [Readonly]
        [SerializeField]
        protected float fuel;


        /// <summary>Текущее количество топлива в долях от максимального, <c>0 ≤ Fuel ≤ 1</c>.</summary>
        public float Fuel
        {
            get => fuel / maxFuel;
            set
            {
                if (value < 0 || value > 1)
                    throw new System.ArgumentException($"{nameof(Fuel)} must be in range [0,1] but value={value}.");
                fuel = maxFuel * value;
            }
        }

        /// <summary>Максимальное количество топлива, <c>MaxFuel ≥ 1</c>.</summary>
        public float MaxFuel
        {
            get => maxFuel;
            set
            {
                if (value < 1)
                    throw new System.ArgumentException($"{nameof(MaxFuel)} must be greater than 1 but value={value}.");
                maxFuel = value;
            }
        }


        /// <summary>Восстановить максимальное количество топлива.</summary>
        public void ResetFuel() => fuel = MaxFuel;

        /// <summary>Заставляет работать двигатели для получения заданного движения.</summary>
        /// <param name="moveX">Параллельное движение вдоль оси X.</param>
        /// <param name="moveY">Параллельное движение вдоль оси Y.</param>
        /// <param name="moveZ">Параллельное движение вдоль оси Z.</param>
        /// <param name="rotX">Вращение вдоль оси X (pitch - тангаж, нос вниз/вверх).</param>
        /// <param name="rotY">Вращение вдоль оси Y (yaw - рыскание, ност влево/вправо).</param>
        /// <param name="rotZ">Вращение вдоль оси Z (roll - крен, поворот вдоль продольной оси).</param>
        public abstract void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ);

        /// <summary>Выключение всех двигателей.</summary>
        public abstract void Shutdown();


        protected virtual void Start()
        {
            fuel = maxFuel;
        }
    }
}
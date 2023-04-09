﻿using UnityEngine;

namespace Lander.Thrusters
{
    /// <summary>Функционал для согласованного управления системой из нескольких двигателей.</summary>
    public abstract class BaseThrustersController : MonoBehaviour
    {
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


        /// <summary>Количество топлива.</summary>
        public abstract float Fuel { get; }
    }


    /// <summary>Функционал для ручного управления тягой всех двигателей.</summary>
    public abstract class ManualThrustersController : BaseThrustersController
    {
        /// <summary>Здаёт тягу всех двигателей.</summary>
        public abstract void ApplyMovement(float[] thrust);
    }
}
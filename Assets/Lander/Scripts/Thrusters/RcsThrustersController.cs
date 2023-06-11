using System;
using UnityEngine;

namespace Lander.Thrusters
{
    /// <summary>Контроллер маневровых двигателей на пять направлений.</summary>
    public class RcsThrustersController : BaseThrustersController
    {
        [SerializeField]
        private BaseThruster ThrusterUp = null;

        [SerializeField]
        private BaseThruster ThrusterDown = null;

        [SerializeField]
        private BaseThruster ThrusterLeft = null;

        [SerializeField]
        private BaseThruster ThrusterRight = null;

        [SerializeField]
        private BaseThruster ThrusterForward = null;



        /// <summary><inheritdoc cref="BaseThrustersController.ApplyMovement(float, float, float, float, float, float)"/></summary>
        /// <param name="moveX">Вспомогательные горизонтальные двигатели.</param>
        /// <param name="moveY">Вспомогательные вертикальные двигатели.</param>
        /// <param name="moveZ">Основной фронтальный двигатель.</param>
        /// <param name="rotX">НЕ ИСПОЛЬЗУЕТСЯ</param>
        /// <param name="rotY">НЕ ИСПОЛЬЗУЕТСЯ</param>
        /// <param name="rotZ">НЕ ИСПОЛЬЗУЕТСЯ</param>
        public override void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ)
        {
            Burn(ThrusterLeft, -moveX);
            Burn(ThrusterRight, moveX);
            Burn(ThrusterDown, -moveY);
            Burn(ThrusterUp, moveY);
            Burn(ThrusterForward, moveZ);
        }

        public override void Shutdown()
        {
            ThrusterLeft?.Shutdown();
            ThrusterRight?.Shutdown();
            ThrusterDown?.Shutdown();
            ThrusterUp?.Shutdown();
            ThrusterForward?.Shutdown();
        }


        private void Burn(BaseThruster thruster, float thrust)
        {
            if (thruster == null)
                return;

            if (thrust > 0 && fuel >= thrust)
            {
                fuel -= thruster.MaxThrustValue * thrust;
                thruster.Burn(thrust);
            }
            else 
                thruster.Shutdown();
        }
    }
}
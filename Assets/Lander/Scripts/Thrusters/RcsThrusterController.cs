using System;

using UnityEngine;

namespace Lander.Thrusters
{
    public class RcsThrusterController : ManualThrustersController
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

        public float Fuel { get; set; }



        /// <summary><inheritdoc cref="BaseThrustersController.ApplyMovement(float, float, float, float, float, float)"/></summary>
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

        /// <summary><inheritdoc cref="ManualThrustersController.ApplyMovement(float[])"/></summary>
        /// <param name="thrust">Left, Right, Top, Down, Forward</param>
        /// <exception cref="ArgumentException"></exception>
        public override void ApplyMovement(float[] thrust)
        {
            if (thrust.Length != 5)
                throw new ArgumentException("Thrust array must have 5 elements", nameof(thrust));

            Burn(ThrusterLeft, thrust[0]);
            Burn(ThrusterRight, thrust[1]);
            Burn(ThrusterDown, thrust[2]);
            Burn(ThrusterUp, thrust[3]);
            Burn(ThrusterForward, thrust[4]);
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

            if (thrust > 0)
            {
                Fuel -= thruster.MaxThrustValue * thrust;
                thruster.Burn(thrust);
            }
            else
                thruster.Shutdown();
        }
    }
}
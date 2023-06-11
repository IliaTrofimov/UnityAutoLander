using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lander.Thrusters
{
    /// <summary>Основной контроллер двигателей. Управляет маршевыми и маневровыми двигателями одновременно.</summary>
    public class RootThrustersController : BaseThrustersController
    {
        [Space]
        [SerializeField]
        private List<BaseThruster> MainThrusters;

        [Header("RCS Controllers")]
        [SerializeField]
        private RcsThrustersController RcsZNegBot;

        [SerializeField]
        private RcsThrustersController RcsZPosBot;

        [SerializeField]
        private RcsThrustersController RcsZNegTop;

        [SerializeField]
        private RcsThrustersController RcsZPosTop;

        [SerializeField]
        private RcsThrustersController RcsXNegBot;

        [SerializeField]
        private RcsThrustersController RcsXPosBot;

        [SerializeField]
        private RcsThrustersController RcsXNegTop;

        [SerializeField]
        private RcsThrustersController RcsXPosTop;



        public override void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ)
        {
            if (fuel <= 0)
            {
                Shutdown();
                return;
            }
               
            ToggleMain(moveY);

            float yaw = rotY / 4;
            // rotation               yaw      roll
            RcsXPosTop.ApplyMovement(-yaw, 0, -rotZ, 0, 0, 0);
            RcsXNegBot.ApplyMovement(yaw,  0, -rotZ, 0, 0, 0);
            RcsXPosBot.ApplyMovement(-yaw, 0,  rotZ, 0, 0, 0);
            RcsXNegTop.ApplyMovement(yaw,  0,  rotZ, 0, 0, 0);

            // rotation               yaw      pitch
            RcsZPosTop.ApplyMovement(-yaw, 0, -rotX, 0, 0, 0);
            RcsZNegBot.ApplyMovement(yaw,  0, -rotX, 0, 0, 0);
            RcsZPosBot.ApplyMovement(-yaw, 0,  rotX, 0, 0, 0);
            RcsZNegTop.ApplyMovement(yaw,  0,  rotX, 0, 0, 0);


            //fuel += RcsXPosTop.Fuel + RcsXNegTop.Fuel + RcsXPosBot.Fuel + RcsXNegBot.Fuel
            //      + RcsZPosTop.Fuel + RcsZNegTop.Fuel + RcsZPosBot.Fuel + RcsZNegBot.Fuel;
        }

        public override void Shutdown()
        {
            RcsZNegBot.Shutdown();
            RcsZPosBot.Shutdown();
            RcsZNegTop.Shutdown();
            RcsZPosTop.Shutdown();

            RcsXNegBot.Shutdown();
            RcsXPosBot.Shutdown();
            RcsXNegTop.Shutdown();
            RcsXPosTop.Shutdown();

            ToggleMain(0);
        }

        private void ToggleMain(float thrust)
        {
            if (thrust <= 0)
                foreach (var t in MainThrusters)
                    t.Shutdown();
            else
                foreach (var t in MainThrusters)
                    t.Burn(thrust);
            
        }
    }
}
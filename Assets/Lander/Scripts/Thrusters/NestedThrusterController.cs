using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lander.Thrusters
{
    public class NestedThrusterController : BaseThrustersController
    {
        [Header("RCS Controllers")]
        [SerializeField]
        private RcsThrusterController RcsZNegBot;

        [SerializeField]
        private RcsThrusterController RcsZPosBot;

        [SerializeField]
        private RcsThrusterController RcsZNegTop;

        [SerializeField]
        private RcsThrusterController RcsZPosTop;

        [SerializeField]
        private RcsThrusterController RcsXNegBot;

        [SerializeField]
        private RcsThrusterController RcsXPosBot;

        [SerializeField]
        private RcsThrusterController RcsXNegTop;

        [SerializeField]
        private RcsThrusterController RcsXPosTop;

        [Header("Main thrusters")]
        [SerializeField]
        private List<BaseThruster> MainThrusters;

        [Header("Options")]
        [SerializeField]
        [Range(1, 1_00_000)]
        private float MaxFuel = 500_000;

        private float fuel;
        public float Fuel => fuel / MaxFuel;



        public override void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ)
        {
            if (fuel <= 0)
            {
                Shutdown();
                Debug.Log("OUT OF FUEL");
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

        private void Start()
        {
            fuel = MaxFuel;

            RcsZNegBot.Fuel = 0;
            RcsZPosBot.Fuel = 0;
            RcsZNegTop.Fuel = 0;
            RcsZPosTop.Fuel = 0;

            RcsXNegBot.Fuel = 0;
            RcsXPosBot.Fuel = 0;
            RcsXNegTop.Fuel = 0;
            RcsXPosTop.Fuel = 0;
        }

        private static float pos(float x) => x >= 0 ? x : 0;
        private static float neg(float x) => x >= 0 ? 0 : -x;
    }
}
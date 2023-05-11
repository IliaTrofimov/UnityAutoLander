using UnityEngine;

using Lander.Shared;

namespace Lander.AI
{
    public static class Rewards
    {
        public static bool UseLogging;


        public static float inverseSquareZeroed(float x, float xRoot = 1.0f, float xOffset = 0.0f) =>
            2 * (1 / (Mathf.Pow((x - xOffset) / xRoot, 2) + 1) - 0.5f);


        public static float FlyingReward3(MovementInfo movement)
        {
            var r_n = 6 * (Vector3.Dot(movement.Normal, -movement.Velocity.normalized) * 5 - 4);        // r in [-5, 1]
            var r_v = 13 * inverseSquareZeroed(movement.Velocity.magnitude,
                xRoot: 4 + 3 * Mathf.Log(movement.Height + 1),
                xOffset: 1 + 3 * Mathf.Log(Mathf.Pow(movement.Height, 2) + 1)
            );                                                                                           // r in [-1, 1]
            var r_w = -4*movement.AngularVelocity.magnitude;// r in [-5, 0]
            var r = r_v + r_w + r_n;                                                                     // R in [-6, 4] 

            if (UseLogging)
                Debug.Log($"FlyingReward3 = r_vel:{r_v,3:F2} + r_w:{r_w,3:F2} + r_norm:{r_n,3:F2} = {r,3:F2}");

            return r;
        }

        public static float FlyingReward2(MovementInfo movement)
        {
            var r_n = 10*(Vector3.Dot(movement.Normal, -movement.Velocity.normalized) * 3 - 2);          // r in [-5, 1]
            var r_v = 10*inverseSquareZeroed(movement.Velocity.magnitude, xRoot: 20, xOffset: 10);     // r in [-1, 1]
            var r_w = -movement.AngularVelocity.magnitude;// r in [-5, 0]
            var r = r_v + r_w + r_n;                                                                      // R in [-6, 4] 

            if (UseLogging)
                Debug.Log($"FlyingReward2 = r_vel:{r_v,3:F2} + r_w:{r_w,3:F2} + r_norm:{r_n,3:F2} = {r,3:F2}");

            return r;
        }

        public static float FlyingReward(MovementInfo movement)
        {
            var r_n = Vector3.Dot(movement.Normal, -movement.Velocity.normalized)*3 - 2;            // r in [-5, 1]
            var r_v = inverseSquareZeroed(movement.Velocity.magnitude, xRoot: 20);                  // r in [-1, 1]
            var r_w = 2 * (inverseSquareZeroed(movement.AngularVelocity.magnitude, xRoot: 10) - 1); // r in [-5, 0]
            var r_up = Vector3.Dot(movement.Normal, Vector3.up) * 3 - 2;                            // r in [-5, 1]
            var r_down = (Mathf.Atan(-movement.Velocity.y) + 3 * inverseSquareZeroed(movement.Velocity.y, 7, -6.5f)) / 4; // r in [-1, 1]
            var r = r_n + r_v + r_w + r_up + r_down;                                                 // R in [-17, 4] 

            if (UseLogging)
                Debug.Log($"FlyingReward = r_norm:{r_n:F1} + r_vel:{r_v:F1} + r_w:{r_w:F1} + r_vert:{r_up:F1}  + r_desc:{r_down:F1} = {r:F2}");

            return r;        
        }

        public static float FixationReward(MovementInfo movement)
        {
            var r_up = Vector3.Dot(movement.Normal, Vector3.up) * 3 - 2;                            // r in [-5, 1]
            var r_v = inverseSquareZeroed(movement.Velocity.magnitude, xRoot: 0.5f);                // r in [-1, 1]
            var r = r_v + r_up;                                                                     // R in [-6, 2] 

            if (UseLogging)
                Debug.Log($"FixationReward = r_v:{r_v:F1} + r_up:{r_up:F1} = {r:F2}");

            return r;
        }

        public static float TouchedReward(MovementInfo movement)
        {
            var r_up = Vector3.Dot(movement.Normal, Vector3.up) * 3 - 2;                            // r in [-5, 1]
            var r_v = inverseSquareZeroed(movement.Velocity.magnitude, xRoot: 5);                   // r in [-1, 1]
            var r = r_v + r_up;                                                                     // R in [-6, 2]

            if (UseLogging)
                Debug.Log($"TouchedReward = r_v:{r_v:F1} + r_up:{r_up:F1} = {r:F2}");

            return r;
        }

        public static float FatalReward()
        {
            var r = -100;  
            if (UseLogging)
                Debug.Log($"FatalReward = {r:F2}");
            return r;
        }

        public static float LandedReward()
        {
            var r = 100;
            if (UseLogging)
                Debug.Log($"LandedReward = {r:F2}");
            return r;
        }
    }
}
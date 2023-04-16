using UnityEngine;

using Lander.Shared;

namespace Lander.AI
{
    public static class Rewards
    {
        public static bool UseLogging;


        public static float inverseSquareZeroed(float x, float xRoot = 1.0f, float xOffset = 0.0f) =>
            2 * (1 / (Mathf.Pow((x - xOffset) / xRoot, 2) + 1) - 0.5f);

        

        public static float FlyingReward(MovementInfo movement)
        {
            var r_n = Vector3.Dot(movement.Normal, -movement.Velocity.normalized)*3 - 2;            // r in [-5, 1]
            var r_v = inverseSquareZeroed(movement.Velocity.magnitude, xRoot: 20);                  // r in [-1, 1]
            var r_w = 2 * (inverseSquareZeroed(movement.AngularVelocity.magnitude, xRoot: 10) - 1); // r in [-5, 0]
            var r_up = Vector3.Dot(movement.Normal, Vector3.up) * 3 - 2;                            // r in [-5, 1]
            var r_down = (Mathf.Atan(-movement.Velocity.y) + 3 * inverseSquareZeroed(movement.Velocity.y, 4, -4)) / 4; // r in [-1, 1]
            var r = r_n + r_v + r_w + r_up + r_down;                                                 // R in [-17, 4] 

            if (UseLogging)
                Debug.Log($"FlyingReward = r_n:{r_n:F1} + r_v:{r_v:F1} + r_w:{r_w:F1} + r_up:{r_up:F1}  + r_down:{r_down:F1} = {r:F2}");

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
            var r = -1000;  
            if (UseLogging)
                Debug.Log($"FatalReward = {r:F2}");
            return r;
        }

        public static float LandedReward()
        {
            var r = 1000;
            if (UseLogging)
                Debug.Log($"LandedReward = {r:F2}");
            return r;
        }
    }
}
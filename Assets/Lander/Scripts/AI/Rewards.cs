using UnityEngine;

namespace Lander.AI
{

    public static class Rewards
    {
        public static float inverseSquareZeroed(float x, float xRoot = 1.0f, float xOffset = 0.0f) =>
            2 * (1 / (Mathf.Pow((x - xOffset) / xRoot, 2) + 1) - 0.5f);
    }
}
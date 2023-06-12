using UnityEngine;

namespace Lander.Shared
{
    public static class MathUtils
    {
        public static readonly Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);
        public static readonly System.Random random = new();


        /// <summary>Функция <c>pos(x) = {x, x&gt;0 v 0, иначе}</c></summary>
        public static float Pos(float x) => x >= 0 ? x : 0;

        /// <summary>Функция <c>pos(x) = {0, x&gt;0 v -x, иначе}</c></summary>
        public static float Neg(float x) => x >= 0 ? 0 : -x;

        /// <summary>Функция <c>invSqr(x,r,o) = 2*(1/(((x - o)/r)^2 + 1) - 0.5)</c> </summary>
        public static float InvSqr(float x, float xRoot = 1.0f, float xOffset = 0.0f) =>
            2 * (1 / (Mathf.Pow((x - xOffset) / xRoot, 2) + 1) - 0.5f);

        public static bool IsZero(this Vector3 vector) =>
            vector.x == 0 && vector.y == 0 && vector.z == 0;

        public static bool IsZero(this Vector2 vector) =>
            vector.x == 0 && vector.y == 0;

        public static string ShortFormat(this float x) =>
            Mathf.Abs(x) < 10 ? $"{x:F2}" : 
            Mathf.Abs(x) < 100 ? $"{x:F1}" : $"{x:F0}";

        public static string ShortFormat(this Vector3 v) =>
            $"<{v.x.ShortFormat()}, {v.y.ShortFormat()}, {v.z.ShortFormat()}>";

        public static Vector3 RandomVector3(Vector3 mean, float dispersion = 1.0f) =>
            RandomVector3() * dispersion + mean;

        public static Vector3 RandomVector3(float mean, float dispersion = 1.0f) =>
            RandomVector3() * dispersion + Vector3.one * mean;

        public static Vector3 RandomVector3() =>
            new Vector3((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
    }
}
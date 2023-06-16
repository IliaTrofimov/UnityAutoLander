using System;
using UnityEngine;

namespace Lander.Shared
{
    public enum Axis { X = 0, Z = 1, Y = 2 }

    public enum Direction
    {
        Positive = 1,
        Negative = -1
    }

    public enum AxisOffset
    {
        Coaxial = 1,
        Offseted = -1
    }

    public static class AxisUtils
    {
        public static Vector3 GetVector(Axis axis, Direction direction)
        {
            switch (axis)
            {
                case Axis.X: return new Vector3((int)direction, 0, 0);
                case Axis.Y: return new Vector3(0, (int)direction, 0);
                case Axis.Z: return new Vector3(0, 0, (int)direction);
                default: return new Vector3(0, 0, 0);
             }
        }

        public static Color GetColorForAxis(Axis axis)
        {
            switch (axis)
            {
                case Axis.X: return Color.red;
                case Axis.Y: return Color.green;
                case Axis.Z: return Color.blue;
                default:
                    return Color.gray;
            }
        }

    }
}
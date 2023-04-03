using System;
using UnityEngine;

namespace Shared
{
    public static class AxisInfo
    {
        public enum Axis { X, Z, Y }

        public enum Direction
        {
            Positive = 1,
            Negative = -1
        }

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
                case AxisInfo.Axis.X: return Color.red;
                case AxisInfo.Axis.Y: return Color.green;
                case AxisInfo.Axis.Z: return Color.blue;
                default:
                    return Color.gray;
            }
        }

    }
}
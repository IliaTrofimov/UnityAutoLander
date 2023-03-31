using System;

using Utils;

namespace Thrusters
{
    public class ThrusterPosition
    {
        public AxisInfo.Axis Axis { get; set; }
        public AxisInfo.Direction Direction { get; set; }
        public ThrusterPlacement Placement { get; set; }

        public ThrusterPosition(AxisInfo.Axis axis, AxisInfo.Direction direction, ThrusterPlacement placement)
        {
            Axis = axis;
            Direction = direction;
            Placement = placement;
        }

        public override int GetHashCode() => HashCode.Combine(Axis, Direction, Placement);
        public override bool Equals(object obj) => obj is ThrusterPosition p && p.Axis == Axis && p.Direction == Direction && p.Placement == Placement;
        public override string ToString() => $"{(Direction == AxisInfo.Direction.Negative ? '-' : '+')}{Axis}{Placement}";


        public static ThrusterPosition XPositiveTop => new ThrusterPosition(AxisInfo.Axis.X, AxisInfo.Direction.Positive, ThrusterPlacement.Top);
        public static ThrusterPosition XPositiveBot => new ThrusterPosition(AxisInfo.Axis.X, AxisInfo.Direction.Positive, ThrusterPlacement.Bottom);
        public static ThrusterPosition XNegativeTop => new ThrusterPosition(AxisInfo.Axis.X, AxisInfo.Direction.Negative, ThrusterPlacement.Top);
        public static ThrusterPosition XNegativeBot => new ThrusterPosition(AxisInfo.Axis.X, AxisInfo.Direction.Negative, ThrusterPlacement.Bottom);

        public static ThrusterPosition ZPositiveTop => new ThrusterPosition(AxisInfo.Axis.Z, AxisInfo.Direction.Positive, ThrusterPlacement.Top);
        public static ThrusterPosition ZPositiveBot => new ThrusterPosition(AxisInfo.Axis.Z, AxisInfo.Direction.Positive, ThrusterPlacement.Bottom);
        public static ThrusterPosition ZNegativeTop => new ThrusterPosition(AxisInfo.Axis.Z, AxisInfo.Direction.Negative, ThrusterPlacement.Top);
        public static ThrusterPosition ZNegativeBot => new ThrusterPosition(AxisInfo.Axis.Z, AxisInfo.Direction.Negative, ThrusterPlacement.Bottom);

        public static ThrusterPosition YPositiveTop => new ThrusterPosition(AxisInfo.Axis.Y, AxisInfo.Direction.Positive, ThrusterPlacement.Top);
        public static ThrusterPosition YPositiveBot => new ThrusterPosition(AxisInfo.Axis.Y, AxisInfo.Direction.Positive, ThrusterPlacement.Bottom);
        public static ThrusterPosition YNegativeTop => new ThrusterPosition(AxisInfo.Axis.Y, AxisInfo.Direction.Negative, ThrusterPlacement.Top);
        public static ThrusterPosition YNegativeBot => new ThrusterPosition(AxisInfo.Axis.Y, AxisInfo.Direction.Negative, ThrusterPlacement.Bottom);
    } 
}
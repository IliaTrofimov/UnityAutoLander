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
    } 
}
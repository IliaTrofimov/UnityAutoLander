using System;
using Utils;

namespace Thrusters
{

    public class ThrustInfo
    {
        public ThrusterPosition Position { get; set; }
        public float Thrust { get; set; }

        public ThrustInfo(ThrusterPosition position, float thrust)
        {
            Position = position;
            Thrust = thrust;
        }

        public ThrustInfo(AxisInfo.Axis axis, AxisInfo.Direction direction, ThrusterPlacement placement, float thrust)
        {
            Position = new ThrusterPosition(axis, direction, placement);
            Thrust = thrust;
        }
    }

}
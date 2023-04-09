using Lander.Shared;

namespace Lander.hrusters
{
    public class ThrustInfo
    {
        public PositionOnSpacecraft Position { get; set; }
        public float Thrust { get; set; }
        public string Uid { get; set; }

        public ThrustInfo(AxisInfo.Axis axis, AxisInfo.Direction direction, Placement placement,  float thrust, string uid)
        {
            Position = new PositionOnSpacecraft(axis, direction, placement);
            Thrust = thrust;
            Uid = uid;
        }
    }
}
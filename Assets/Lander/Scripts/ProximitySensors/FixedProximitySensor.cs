using UnityEngine;
using Lander.Shared;

namespace Lander.ProximitySensors
{
    /// <summary>Датчик, измерящий расстояние вдоль заданной оси в мировой системе координат.</summary>
    public sealed class FixedProximitySensor : BaseProximitySensor, ILabeled<int>
	{
        [SerializeField]
        private AxisInfo.Axis Axis = AxisInfo.Axis.Y;

        [SerializeField]
        private AxisInfo.Direction Direction = AxisInfo.Direction.Negative;

        private void Start()
        {
            direction = AxisInfo.GetVector(Axis, Direction);
            sensorLaserColor = Color.blue;
        }

        public int GetLabel()
        {
            return 0;
        }
    }
}
using System;
using UnityEngine;

using Shared;

namespace ProximitySensors
{
    /// <summary>Датчик высоты.</summary>
    public sealed class HeightSensor : BaseProximitySensor, ILabeled<int>
    {
        private AxisInfo.Axis Axis = AxisInfo.Axis.Y;
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
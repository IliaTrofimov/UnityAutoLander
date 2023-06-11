using System;
using UnityEngine;

using Lander.Shared;

namespace Lander.ProximitySensors
{
    /// <summary>Датчик высоты.</summary>
    public sealed class HeightSensor : BaseProximitySensor
    {
        private void Start()
        {
            direction = Vector3.down;
            sensorLaserColor = Color.blue;
        }
    }
}
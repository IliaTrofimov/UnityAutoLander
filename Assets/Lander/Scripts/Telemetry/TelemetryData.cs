using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Thrusters;

namespace Telemetry
{

    public class TelemetryData
    {
        public DateTime Time { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 EulerAngles { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
        public float Fuel { get; set; }
        public float Height { get; set; }
        public HashSet<SensorInfo> Sensors { get; set; }
        public HashSet<ThrustInfo> ThrustInfos { get; set; }

        public TelemetryData()
        {
            Time = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Time:HH:mm:ss.fff};" +
                   $"P;{Position.x:F3};{Position.y:F3};{Position.z:F3};" +
                   $"V;{Velocity.x:F3};{Velocity.y:F3};{Velocity.z:F3};" +
                   $"A;{EulerAngles.x:F3};{EulerAngles.y:F3};{EulerAngles.z:F3};" +
                   $"AV;{AngularVelocity.x:F3};{AngularVelocity.y:F3};{AngularVelocity.z:F3};" +
                   $"F;{Fuel:F3};" +
                   $"H;{Height:F3};";
        }
    }
}
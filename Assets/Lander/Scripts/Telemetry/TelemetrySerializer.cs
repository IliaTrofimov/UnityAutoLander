using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Shared;

namespace Telemetry
{
    public class TelemetryData
    {
        public TimeSpan Time { get; set; }
        public MovementInfo Movement { get; set; }
        public float Fuel { get; set; }
        public Dictionary<PositionOnSpacecraft, float> SensorsData { get; set; } = new();
        public Dictionary<PositionOnSpacecraft, float> ThrustData { get; set; } = new();


        public override string ToString()
        {
            var euler = Movement.Quaternion.eulerAngles;
            return $"{Time.TotalSeconds:F3};" +
                   $"P;{Movement.Position.x:F3};{Movement.Position.y:F3};{Movement.Position.z:F3};" +
                   $"V;{Movement.Velocity.x:F3};{Movement.Velocity.y:F3};{Movement.Velocity.z:F3};" +
                   $"A;{euler.x:F3};{euler.y:F3};{euler.z:F3};" +
                   $"Av;{Movement.AngularVelocity.x:F3};{Movement.AngularVelocity.y:F3};{Movement.AngularVelocity.z:F3};" +
                   $"F;{Fuel:F3};" +
                   $"H;{Movement.Height:F3};" +
                   $"S_{SensorsData.Count};{string.Join(';', SensorsData.Values)};" +
                   $"T_{ThrustData.Count};{string.Join(';', ThrustData.Values)}";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using Shared;
using System.IO;
using System.Data;

namespace Telemetry
{
    public class ConsoleTelemetryLogger : MonoBehaviour
    {
        [Range(0, 10)]
        public float LoggingPeriodSeconds;
        private DateTime startTime = DateTime.Now;
        private DateTime previousLog = DateTime.Now;
        protected TelemetryData data = new();

        public ConsoleTelemetryLogger SetFuel(float fuel)
        {
            data.Fuel = fuel;
            return this;
        }

        public ConsoleTelemetryLogger SetMovement(MovementInfo movement)
        {
            data.Movement = movement;
            return this;
        }

        public ConsoleTelemetryLogger SetMovement(Vector3? position = null,
            Vector3? eulerAngles = null,
            Vector3? velocity = null,
            Vector3? angularVelocity = null,
            float? height = null,
            bool? isCollided = null)
        {
            if (position != null) data.Movement.Position = position.Value;
            if (eulerAngles != null) data.Movement.EulerAngles = eulerAngles.Value;
            if (velocity != null) data.Movement.Velocity = velocity.Value;
            if (angularVelocity != null) data.Movement.AngularVelocity = angularVelocity.Value;
            if (height != null) data.Movement.Height = height.Value;
            if (isCollided != null) data.Movement.IsCollided = isCollided.Value;

            return this;
        }

        public ConsoleTelemetryLogger SetSensorsData(Dictionary<PositionOnSpacecraft, float> sensorsData)
        {
            foreach (var kvp in sensorsData)
                if (!data.SensorsData.TryAdd(kvp.Key, kvp.Value))
                    data.SensorsData[kvp.Key] = kvp.Value;

            return this;
        }

        public ConsoleTelemetryLogger SetSensorsData(PositionOnSpacecraft pos, float distance)
        {
            if (!data.SensorsData.TryAdd(pos, distance))
                data.SensorsData[pos] = distance;

            return this;
        }

        public ConsoleTelemetryLogger SetThrustData(Dictionary<PositionOnSpacecraft, float> thrustData)
        {
            foreach (var kvp in thrustData)
                if (!data.ThrustData.TryAdd(kvp.Key, kvp.Value))
                    data.ThrustData[kvp.Key] = kvp.Value;

            return this;
        }

        public ConsoleTelemetryLogger SetThrustData(PositionOnSpacecraft pos, float thrust)
        {
            if (!data.ThrustData.TryAdd(pos, thrust))
                data.ThrustData[pos] = thrust;

            return this;
        }

        protected virtual async Task LogData()
        {
            Debug.Log(data);
        }

        private void FixedUpdate()
        {
            if ((DateTime.Now - previousLog).TotalSeconds > LoggingPeriodSeconds)
            {
                data.Time = DateTime.Now - startTime;
                LogData();
                previousLog = DateTime.Now;
            }
        }

    }
}
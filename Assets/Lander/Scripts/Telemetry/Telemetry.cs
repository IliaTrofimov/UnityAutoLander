using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using Thrusters;
using System.IO;
using Lander;
using Utils;
using CraftState;

namespace Telemetry
{

    public class Telemetry : MonoBehaviour
    {
        public string LogFile;

        private TelemetryData currentData = new();
        private List<TelemetryData> data = new();
        private HashSet<ThrustInfo> thrusters = new();


        public void OnLanderMovement(MovementInfo info)
        {
            currentData.Velocity = info.Velocity;
            currentData.EulerAngles = info.EulerAngles;
            currentData.Position = info.Position;
            currentData.AngularVelocity = info.AngularVelocity;
        }

        public void OnFuelChanged(float fuel)
        {
            currentData.Fuel = fuel;
        }

        public void OnThrusterBurning(ThrustInfo info)
        {
            if (thrusters.TryGetValue(info, out ThrustInfo actual))
                actual.Thrust = info.Thrust;
            else
                thrusters.Add(info);
        }
        

        private void OnApplicationQuit()
        {
            if (string.IsNullOrEmpty(LogFile))
                return;

            using StreamWriter sw = new StreamWriter(LogFile);
            foreach (var item in data)
                sw.WriteLine(item);
            Debug.Log("Log saved");
        }
    }

}
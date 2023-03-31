using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Utils;

namespace Thrusters
{

    public class ThrustersController : IThrustersController
    {
        [Range(0, 100_000)]
        public float Fuel = 50_000;
        public UnityEvent<float> FuelChangedEvent;

        private Dictionary<ThrusterPosition, List<Thruster>> thrusters = new();


        public ThrustersController(Thruster[] thrusters)
        {
            foreach (var t in thrusters)
            {
                var pos = new ThrusterPosition(t.Axis, t.Direction, t.Placement);
                if (this.thrusters.TryAdd(pos, new List<Thruster>() { t }))
                    this.thrusters[pos].Add(t);
            }
        }


        public void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ)
        {
            if (Fuel <= 0)
            {
                foreach (var kvp in thrusters)
                    Shutdown(kvp.Value);
                Debug.Log("No fuel");
                return;
            }

            Burn(thrusters[ThrusterPosition.YPositiveBot], moveY);
            BurnOpposite(thrusters[ThrusterPosition.XPositiveTop], thrusters[ThrusterPosition.XNegativeTop], rotZ);
            BurnOpposite(thrusters[ThrusterPosition.XPositiveBot], thrusters[ThrusterPosition.XNegativeBot], -rotZ);
            BurnOpposite(thrusters[ThrusterPosition.ZPositiveTop], thrusters[ThrusterPosition.ZNegativeTop], rotX);
            BurnOpposite(thrusters[ThrusterPosition.ZPositiveBot], thrusters[ThrusterPosition.ZNegativeBot], -rotX);

            FuelChangedEvent?.Invoke(Fuel);
        }

        private void BurnOpposite(IEnumerable<Thruster> pos, IEnumerable<Thruster> neg, float thrust)
        {
            if (thrust == 0)
            {
                Shutdown(pos);
                Shutdown(neg);
            }
            else if (thrust > 0)
            {
                Shutdown(neg);
                Burn(pos, thrust);
            }
            else
            {
                Shutdown(pos);
                Burn(neg, -thrust);
            }
        }

        private void Burn(IEnumerable<Thruster> thrusters, float thrust)
        {
            foreach (var t in thrusters)
            {
                Fuel -= t.BaseThrustValue * thrust;
                if (Fuel > 0)
                    t.Burn(thrust);
            }  
        }

        private void Shutdown(IEnumerable<Thruster> thrusters)
        {
            foreach (var t in thrusters)
                t.Shutdown();
        }
    }
}
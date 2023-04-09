using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Lander.Shared;
using Lander.ProximitySensors;

namespace Lander.Thrusters
{
    /// <summary>Контроллер двигателей.</summary>
    public class ThrustersController : ManualThrustersController
    {
        private Dictionary<PositionOnSpacecraft, List<Thruster>> thrusters = new();

        [Range(0, 100_000)]
        [SerializeField]
        private float fuel = 50_000;

        public override float Fuel => fuel;


        public override void ApplyMovement(float[] thrust)
        {
            foreach (var (thruster, val) in thrusters.Values.SelectMany(t => t).Zip(thrust, (Thruster thruster, float val) => (thruster, val)))
            {
                if (fuel > 0)
                {
                    fuel -= thruster.MaxThrustValue * val;
                    thruster.Burn(val);
                }  
                else
                    thruster.Shutdown();
            }
        }

        public override void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ)
        {
            if (fuel <= 0)
            {
                foreach (var kvp in thrusters)
                    Shutdown(kvp.Value);
                Debug.Log("No fuel");
                return;
            }

            Burn(thrusters[PositionOnSpacecraft.YPositiveBot], moveY);
            BurnOpposite(thrusters[PositionOnSpacecraft.XPositiveTop], thrusters[PositionOnSpacecraft.XNegativeTop], rotZ);
            BurnOpposite(thrusters[PositionOnSpacecraft.XPositiveBot], thrusters[PositionOnSpacecraft.XNegativeBot], -rotZ);
            BurnOpposite(thrusters[PositionOnSpacecraft.ZPositiveTop], thrusters[PositionOnSpacecraft.ZNegativeTop], rotX);
            BurnOpposite(thrusters[PositionOnSpacecraft.ZPositiveBot], thrusters[PositionOnSpacecraft.ZNegativeBot], -rotX);
        }

        public override void Shutdown()
        {
            Shutdown(thrusters.SelectMany(kvp => kvp.Value));
        }



        private void Start()
        {
            foreach (var t in GetComponentsInChildren<Thruster>())
                if (!this.thrusters.TryAdd(t.GetLabel().label, new List<Thruster>() { t }))
                    this.thrusters[t.GetLabel().label].Add(t);

            foreach (var t in thrusters.Values)
                t.Sort((Thruster a, Thruster b) => a.GetLabel().order.CompareTo(b.GetLabel().order));
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
            if (thrust > 0)
            {
                foreach (var t in thrusters)
                {
                    fuel -= t.MaxThrustValue * thrust;
                    if (fuel > 0)
                        t.Burn(thrust);
                    else
                        t.Shutdown();
                }
            }
            else
                Shutdown(thrusters);
        }

        private void Shutdown(IEnumerable<Thruster> thrusters)
        {
            foreach (var t in thrusters)
                t.Shutdown();
        }
    }
}
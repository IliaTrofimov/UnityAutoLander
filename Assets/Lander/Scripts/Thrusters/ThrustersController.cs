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
        private float maxFuel = 50_000;

        private float fuel;

        public float Fuel
        {
            get => fuel / maxFuel;
            set
            {
                if (value > 1)
                    value = 1;
                else if (value < 0)
                    value = 0;
                fuel = maxFuel * value;
            }
        }

        public void ResetFuel() => Fuel = 1;

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

            Burn(thrusters[PositionOnSpacecraft.YNegativeBot], -moveY);

            Burn(thrusters[PositionOnSpacecraft.ZPositiveBot], -moveZ);
            Burn(thrusters[PositionOnSpacecraft.ZPositiveTop], -moveZ);
            Burn(thrusters[PositionOnSpacecraft.ZNegativeBot], -moveZ);
            Burn(thrusters[PositionOnSpacecraft.ZNegativeTop], -moveZ);

            Burn(thrusters[PositionOnSpacecraft.XPositiveBot], -moveX);
            Burn(thrusters[PositionOnSpacecraft.XPositiveTop], -moveX);
            Burn(thrusters[PositionOnSpacecraft.XNegativeBot], -moveX);
            Burn(thrusters[PositionOnSpacecraft.XNegativeTop], -moveX);

            Burn(thrusters[PositionOnSpacecraft.XPositiveBotOff], rotY);
            Burn(thrusters[PositionOnSpacecraft.XPositiveTopOff], rotY);
            Burn(thrusters[PositionOnSpacecraft.XNegativeBotOff], -rotY);
            Burn(thrusters[PositionOnSpacecraft.XNegativeTopOff], -rotY);

            Burn(thrusters[PositionOnSpacecraft.ZPositiveBotOff], -rotY);
            Burn(thrusters[PositionOnSpacecraft.ZPositiveTopOff], -rotY);
            Burn(thrusters[PositionOnSpacecraft.ZNegativeBotOff], rotY);
            Burn(thrusters[PositionOnSpacecraft.ZNegativeTopOff], rotY);


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
            fuel = maxFuel;

            foreach (var t in GetComponentsInChildren<Thruster>())
                if (!this.thrusters.TryAdd(t.GetLabel().label, new List<Thruster>() { t }))
                    this.thrusters[t.GetLabel().label].Add(t);

            //foreach (var t in thrusters.Values)
            //   t.Sort((Thruster a, Thruster b) => a.GetLabel().order.CompareTo(b.GetLabel().order));
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
                Shutdown(pos);
                Burn(neg, thrust);
            }
            else
            {
                Shutdown(neg);
                Burn(pos, -thrust);
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
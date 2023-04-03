using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Shared;

namespace Thrusters
{
    /// <summary>Контроллер двигателей.</summary>
    public class ThrustersController : MonoBehaviour, IThrustersController
    {
        private Dictionary<PositionOnSpacecraft, List<Thruster>> thrusters = new();

        [Range(0, 100_000)]
        public float Fuel = 50_000;

        public void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ)
        {
            if (Fuel <= 0)
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

        public void Shutdown()
        {
            Shutdown(thrusters.SelectMany(kvp => kvp.Value));
        }



        private void Start()
        {
            var tr = GetComponentsInChildren<Thruster>();
            foreach (var t in tr)
                if (!this.thrusters.TryAdd(t.Position, new List<Thruster>() { t }))
                    this.thrusters[t.Position].Add(t);
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
                    Fuel -= t.MaxThrustValue * thrust;
                    if (Fuel > 0)
                        t.Burn(thrust);
                    else
                        t.Shutdown();
                }
            }
        }

        private void Shutdown(IEnumerable<Thruster> thrusters)
        {
            foreach (var t in thrusters)
                t.Shutdown();
        }
    }
}
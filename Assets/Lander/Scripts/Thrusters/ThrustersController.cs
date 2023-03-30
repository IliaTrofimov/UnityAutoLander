using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using Utils;
using UnityEditor.Experimental.GraphView;

namespace Thrusters
{

    public class ThrustersController : MonoBehaviour, IThrustersController
    {
        [Range(0, 1_000_000)]
        public float Fuel = 50_000f;
        public UnityEvent<float> FuelChangedEvent;

        private Thruster[] allThrusters;
        private (Thruster[] pos, Thruster[] neg) topXThrusters;
        private (Thruster[] pos, Thruster[] neg) botXThrusters;
        private (Thruster[] pos, Thruster[] neg) topZThrusters;
        private (Thruster[] pos, Thruster[] neg) botZThrusters;

        private Thruster[] mainThrusters;

        private Rigidbody body;
        public Rigidbody Body => body;


        private void Start()
        {
            body = gameObject.GetComponent<Rigidbody>();
            allThrusters = gameObject.GetComponentsInChildren<Thruster>();

            topXThrusters = GetThrustersPair(allThrusters, ThrusterPlacement.Top, AxisInfo.Axis.X);
            botXThrusters = GetThrustersPair(allThrusters, ThrusterPlacement.Bottom, AxisInfo.Axis.X);
            topZThrusters = GetThrustersPair(allThrusters, ThrusterPlacement.Top, AxisInfo.Axis.Z);
            botZThrusters = GetThrustersPair(allThrusters, ThrusterPlacement.Bottom, AxisInfo.Axis.Z);

            mainThrusters = allThrusters.Where(t => t.Axis == AxisInfo.Axis.Y && t.Direction == AxisInfo.Direction.Positive).ToArray();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                //  m_Jump = true;
                ApplyMovement(0, 100, 0, 0, 0, 0);
            }
        }

        public void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ)
        {
            if (Fuel <= 0)
            {
                Debug.Log("No fuel");
                Shutdown(allThrusters);
                return;
            }

            Burn(mainThrusters, moveY);
            BurnOpposite(topXThrusters, rotZ);
            BurnOpposite(botXThrusters, -rotZ);
            BurnOpposite(topZThrusters, -rotX);
            BurnOpposite(botZThrusters, rotX);

            FuelChangedEvent.Invoke(Fuel);
        }

        private void BurnOpposite((Thruster[] pos, Thruster[] neg) thrusters, float thrust)
        {
            if (thrust == 0)
            {
                Shutdown(thrusters.pos);
                Shutdown(thrusters.neg);
            }
            else if (thrust > 0)
            {
                Shutdown(thrusters.neg);
                Burn(thrusters.pos, thrust);
            }
            else
            {
                Shutdown(thrusters.pos);
                Burn(thrusters.neg, -thrust);
            }
        }

        private void Burn(Thruster[] thrusters, float thrust)
        {
            foreach (var t in thrusters)
            {
                t.Burn(thrust);
                Fuel -= t.BaseThrustValue * thrust;
            }  
        }

        private void Shutdown(Thruster[] thrusters)
        {
            foreach (var t in thrusters)
                t.Shutdown();
        }

        private (Thruster[] pos, Thruster[] neg) GetThrustersPair(Thruster[] thrusters, ThrusterPlacement pos, AxisInfo.Axis axis)
        {
            return (
                thrusters.Where(t => t.Axis == axis && t.Placement == pos && t.Direction == AxisInfo.Direction.Positive).ToArray(),
                thrusters.Where(t => t.Axis == axis && t.Placement == pos && t.Direction == AxisInfo.Direction.Negative).ToArray()
            );
        }
    }
}
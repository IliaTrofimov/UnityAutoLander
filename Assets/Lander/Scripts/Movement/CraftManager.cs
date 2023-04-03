using System;
using System.Linq;

using UnityEngine;

using Shared;
using Thrusters;
using CraftState;
using ProximitySensors;
using Telemetry;

namespace Movement
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody), typeof(ThrustersController))]
    public class CraftManager : MonoBehaviour
    {
        [Range(0.1f, 10)]
        public float MaxMovementAfterTouch = 5;

        [Range(-1, 1)]
        public float MaxRotationAfterTouch = 20;

        [Range(0, 60)]
        public float MaxTocuhVelocity = 40;

        [Range(0.01f, 10)]
        public float StandStillSeconds = 2;

        private BaseState state;
        private Rigidbody body;
        private HeightSensor heightSensor;
        private ProximitySensor[] directionalSensors;
        private Thruster[] thrusters;
        private ThrustersController thrustersController;
        private ConsoleTelemetryLogger logger;
        private bool isCollided;

        private MovementInfo Movement => new MovementInfo()
        {
            Position = body.worldCenterOfMass,
            Velocity = body.velocity,
            Quaternion = body.transform.rotation,
            Normal = body.transform.up,
            AngularVelocity = body.angularVelocity,
            Height = heightSensor.Distance,
            IsCollided = isCollided
        };


        private void Start()
        {
            BaseState.MaxTocuhVelocity = MaxTocuhVelocity;
            BaseState.MaxMovementAfterTouch = MaxMovementAfterTouch;
            BaseState.MaxRotationAfterTouch = MaxRotationAfterTouch;
            BaseState.StandStillSeconds = StandStillSeconds;

            body = gameObject.GetComponent<Rigidbody>();
            heightSensor = gameObject.GetComponentInChildren<HeightSensor>();
            directionalSensors = gameObject.GetComponentsInChildren<ProximitySensor>();
            thrustersController = gameObject.GetComponent<ThrustersController>();
            thrusters = gameObject.GetComponentsInChildren<Thruster>();

            state = new FlyingState(Movement);
            Debug.Log(state);

            try
            {
                logger = gameObject.GetComponent<ConsoleTelemetryLogger>();
            }
            catch
            {
                logger = null;      
            }
        }

        private void FixedUpdate()
        {
            var movement = Movement;
            state = state.NextState(movement);
            if (state.IsStateChanged)
                Debug.Log(state);

            if (logger != null)
            {
                logger.SetMovement(movement).SetFuel(thrustersController.Fuel);
                foreach (var s in directionalSensors)
                    logger.SetSensorsData(s.Position, s.Distance);
                foreach (var t in thrusters)
                    logger.SetThrustData(t.Position, 0);
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            isCollided = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            isCollided = false;
        }
    }
}
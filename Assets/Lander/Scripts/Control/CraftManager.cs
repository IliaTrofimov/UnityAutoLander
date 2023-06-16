using System;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;
using UnityEditor;

using Lander.Shared;
using Lander.Thrusters;
using Lander.CraftState;
using Lander.ProximitySensors;
using System.Collections.Generic;


namespace Lander.Control
{

    /// <summary>Контроллер, управляющий состоянием космического корабля.</summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class CraftManager : MonoBehaviour
    {
        #region Editor fields
        [Header("Sensors")]
        [SerializeField]
        private HeightSensor heightSensor;

        [Header("Logging")]
        [SerializeField]
        private bool useTelemetryLogging;

        [SerializeField]
        private string telemetryLogsFile;

        [Header("Craft parameters")]
        [SerializeField]
        private StateSettings stateSettings;

        [SerializeField]
        private Vector3 centerOfMass;

        #endregion

        private DateTime? lastExplosion;
        private bool isExploded;
        private Rigidbody body;
        private MovementInfo movement;
        private BaseState state;
        private Logger<BaseState> stateLogger;

        public BaseState State => state;
        public StateSettings StateSettings => stateSettings;
        public string Name => gameObject.name;
       

        public void Reset()
        {
            state = new FlyingState(ResetMovement(), settings: stateSettings).NextState(movement); 
        }

        private MovementInfo ResetMovement(bool isCollided)
        {
            if (movement == null)
                movement = new MovementInfo();

            movement.IsCollided = isCollided;
            movement.Position = body.worldCenterOfMass;
            movement.Velocity = body.velocity;
            movement.Normal = body.transform.up;
            movement.AngularVelocity = body.angularVelocity;
            movement.Height = heightSensor == null
                ? float.PositiveInfinity
                : heightSensor.Distance;

            return movement;
        }

        private MovementInfo ResetMovement()
        {
            if (movement == null)
                movement = new MovementInfo();

            movement.Position = body.worldCenterOfMass;
            movement.Velocity = body.velocity;
            movement.Normal = body.transform.up;
            movement.AngularVelocity = body.angularVelocity;
            movement.Height = heightSensor == null
                ? float.PositiveInfinity
                : heightSensor.Distance;

            return movement;
        }

        private void LogState()
        {
            if (useTelemetryLogging)
               stateLogger.Log(state);
        }

        private void Start()
        {
            if (useTelemetryLogging)
                stateLogger = new Logger<BaseState>($"{telemetryLogsFile}/{gameObject.name}.csv", 5,
                     s =>
                        $"{s.Movement.Position.x:F3};{s.Movement.Position.y:F3};{s.Movement.Position.z:F3};{s.Movement.Height:F3};" +
                        $"{s.Movement.Normal.x:F3};{s.Movement.Normal.y:F3};{s.Movement.Normal.z:F3};" +
                        $"{s.Movement.Velocity.x:F3};{s.Movement.Velocity.y:F3};{s.Movement.Velocity.z:F3};" +
                        $"{s.Movement.AngularVelocity.x:F3};{s.Movement.AngularVelocity.y:F3};{s.Movement.AngularVelocity.z:F3};" +
                        $"{s.Movement.IsCollided};{s.Name}"
                );

            body = gameObject.GetComponent<Rigidbody>();
            heightSensor = gameObject.GetComponentInChildren<HeightSensor>();
            body.centerOfMass = centerOfMass;

            state = new FlyingState(ResetMovement(), settings: stateSettings).NextState(movement);
            LogState();
        }

        private void FixedUpdate()
        {
            state = state.NextState(ResetMovement());
            LogState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            state = state.NextState(ResetMovement(true));
            LogState();
        }

        private void OnCollisionExit(Collision collision)
        {
            state = state.NextState(ResetMovement(false));
            LogState();
        }

        private void OnApplicationQuit()
        {
            if (useTelemetryLogging)
                stateLogger.ForceLog(state);
        }
    }
}
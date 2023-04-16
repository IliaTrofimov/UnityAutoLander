using System;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

using GlobalShared;
using Lander.Shared;
using Lander.Thrusters;
using Lander.CraftState;
using Lander.ProximitySensors;


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
        private bool showStateLog;

        [SerializeField]
        private bool useMovementLogging;

        [Header("Start parameters")]
        [SerializeField]
        private Vector3 centerOfMass;

        [SerializeField]
        private StateSettings stateSettings;
        #endregion

        private Rigidbody body;
        private MovementInfo movement;
        private BaseState state;
        private GlobalShared.Logger<BaseState> stateLogger;

        public BaseState State => state;
        public StateSettings StateSettings => stateSettings;


        public void Reset()
        {
            state = new FlyingState(ResetMovement(), settings: stateSettings).NextState(movement); 
        }

        private MovementInfo ResetMovement(bool? isCollided = null)
        {
            if (movement == null)
                movement = new MovementInfo();

            movement.IsCollided = isCollided ?? movement.IsCollided;
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
            if (showStateLog && state.IsStateChanged)
                Debug.Log($"{state,-10} {movement}");
            if (useMovementLogging)
               Task.Run(() => stateLogger.LogAsync(state));
        }

        private void Start()
        {
            stateLogger = new Logger<BaseState>($"/Users/iliat/Documents/{gameObject.name}_movement.csv", 100, new TimeSpan(0,1,0),
                 s =>
                    $"{s.Movement.Position.x:F3},{s.Movement.Position.y:F3},{s.Movement.Position.z:F3},{s.Movement.Height:F3}," +
                    $"{s.Movement.Normal.x:F3},{s.Movement.Normal.y:F3},{s.Movement.Normal.z:F3}," +
                    $"{s.Movement.Velocity.x:F3},{s.Movement.Velocity.y:F3},{s.Movement.Velocity.z:F3}," +
                    $"{s.Movement.AngularVelocity.x:F3},{s.Movement.AngularVelocity.y:F3},{s.Movement.AngularVelocity.z:F3}," +
                    $"{s.Movement.IsCollided},{s.Name}"
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
            if (useMovementLogging)
                Task.Run(() => stateLogger.ForceLogAsync());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(body.worldCenterOfMass, 0.2f);
            Gizmos.DrawRay(body.worldCenterOfMass, body.velocity.normalized*2);

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(body.worldCenterOfMass, body.angularVelocity.normalized*2);
        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Lander.Thrusters;
using Lander.CraftState;
using Lander.Control;
using Lander.ProximitySensors;

namespace Lander.AI
{

    /// <summary>Агент ИИ для посадочного модуля.</summary>
    public class AutoLanderAgent : Agent
	{
        [Header("Controls")]
        [SerializeField]
        private ProximitySensorsArray SensorsArray;

        [SerializeField]
        private ThrustersController ThrustersController;

        [SerializeField]
        private CraftManager CraftManager;

        [SerializeField]
        private Rigidbody lander;

        [Header("Logging")]
        [SerializeField]
        private bool ShowRewardsLog;

        [Header("Inital state")]
        [SerializeField]
        private Vector3 initialSpeed;

        [SerializeField]
        private Vector3 initialAngularSpeed;

        [SerializeField]
        private bool ShouldResetScene = true;

        private Vector3 initialPos;
        private Quaternion initialRotation;
        private List<ProximitySensor> sensors;
        private float? startHeight;



        public override void Initialize()
        {
            Rewards.UseLogging = ShowRewardsLog || Rewards.UseLogging;
            initialPos = lander.position;
            initialRotation = lander.transform.rotation;
            initialSpeed = lander.velocity;
            initialAngularSpeed = lander.angularVelocity;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            var state = CraftManager.State;

            // 3 + 3 + 3 + 1 + 1 + 5 = 16
            sensor.AddObservation(state.Movement.Normal.x);
            sensor.AddObservation(state.Movement.Normal.y);
            sensor.AddObservation(state.Movement.Normal.z);
            sensor.AddObservation(state.Movement.Velocity.x);
            sensor.AddObservation(state.Movement.Velocity.y);
            sensor.AddObservation(state.Movement.Velocity.z);
            sensor.AddObservation(state.Movement.AngularVelocity.x);
            sensor.AddObservation(state.Movement.AngularVelocity.y);
            sensor.AddObservation(state.Movement.AngularVelocity.z);
            sensor.AddObservation(ThrustersController.Fuel);
            sensor.AddObservation(state.Movement.Height);
            foreach (var s in SensorsArray.Sensors)
                sensor.AddObservation(s.Distance);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            float up = actionBuffers.ContinuousActions[0] > 0 ? actionBuffers.ContinuousActions[0] : 0;
            float pitch = actionBuffers.ContinuousActions[1];
            float yaw = actionBuffers.ContinuousActions[2];
            float roll = actionBuffers.ContinuousActions[3];

            ThrustersController.ApplyMovement(0, up, 0, pitch, yaw, roll);

            if (CraftManager.State is FinalState)
            {
                if (CraftManager.State is FatalState || ThrustersController.Fuel == 0)
                    AddReward(Rewards.FatalReward());
                else if (CraftManager.State is LandedState)
                    AddReward(Rewards.LandedReward());

                EndEpisode();
            }
            else if (CraftManager.State is FlyingState)
                AddReward(Rewards.FlyingReward(CraftManager.State.Movement));
            else if (CraftManager.State is TouchedState)
                AddReward(Rewards.FlyingReward(CraftManager.State.Movement));
            else if (CraftManager.State is FixationState)
                AddReward(Rewards.FixationReward(CraftManager.State.Movement));
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxis("Jump");
            continuousActionsOut[1] = Input.GetAxis("Pitch");
            continuousActionsOut[2] = Input.GetAxis("Yaw");
            continuousActionsOut[3] = Input.GetAxis("Roll");
        }

        public override void OnEpisodeBegin()
        {
            startHeight = CraftManager.State.Movement.Height;
            if (ShouldResetScene)
            {
                lander.transform.position = initialPos;
                lander.velocity = initialSpeed;
                lander.angularVelocity = initialAngularSpeed;
                lander.transform.rotation = initialRotation;

                ThrustersController.ResetFuel();
                CraftManager.Reset();
            }
            base.OnEpisodeBegin();
        }


        #region Rewards
        
        #endregion
    }
}
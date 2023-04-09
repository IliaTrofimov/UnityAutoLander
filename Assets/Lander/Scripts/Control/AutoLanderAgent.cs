using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

using Lander.Shared;
using Lander.Thrusters;
using Lander.CraftState;
using Lander.ProximitySensors;

namespace Lander.Control
{
    /// <summary>Агент ИИ для посадочного модуля.</summary>
    public class AutoLanderAgent : Agent
	{
        [SerializeField]
        private ProximitySensorsArray SensorsArray;

        [SerializeField]
        private ThrustersController ThrustersController;

        [SerializeField]
        private CraftManager CraftManager;

        private List<ProximitySensor> sensors;
        private float? startHeight;


        public override void Initialize()
        {
            Debug.Log("init");
            sensors = SensorsArray.Sensors.OrderBy(kvp => kvp.Key).SelectMany(kvp => kvp.Value).ToList();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            var state = CraftManager.State;

            // 3 + 3 + 3 + 1 + 1 + 5 = 16
            sensor.AddObservation(state.Movement.Normal);
            sensor.AddObservation(state.Movement.Velocity);
            sensor.AddObservation(state.Movement.AngularVelocity);
            sensor.AddObservation(ThrustersController.Fuel);
            sensor.AddObservation(state.Movement.Height);
            sensor.AddObservation(sensors.Select(s => s.Distance).ToList());

            Debug.Log("Observations -> " + string.Join(",", sensor));
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            float up = actionBuffers.ContinuousActions[0];
            float pitch = actionBuffers.ContinuousActions[1];
            float yaw = actionBuffers.ContinuousActions[2];
            float roll = actionBuffers.ContinuousActions[3];

            Debug.Log($"Actons -> {up}, {pitch}, {yaw}, {roll}");

            ThrustersController.ApplyMovement(0, up, 0, pitch, yaw, roll);
  

            if (CraftManager.State is FatalState)
                FatalReward();
            else if (CraftManager.State is FlyingState)
                FlyingReward(CraftManager.State.Movement);
            else if (CraftManager.State is TouchedState)
                TouchedReward(CraftManager.State.Movement);
            else if (CraftManager.State is FixationState)
                FixationReward(CraftManager.State.Movement);
            else if (CraftManager.State is LandedState)
                LandedReward();
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxis("Jump");
            continuousActionsOut[1] = Input.GetAxis("Pitch");
            continuousActionsOut[2] = Input.GetAxis("Yaw");
            continuousActionsOut[3] = Input.GetAxis("Roll");

            Debug.Log("Heuristic -> " + string.Join(",", continuousActionsOut));
        }

        public override void OnEpisodeBegin()
        {
            Debug.Log("Episode begin!");
            base.OnEpisodeBegin();
        }


        #region Rewards
        private void FlyingReward(MovementInfo movement)
        {
            AddReward(Vector3.Dot(movement.Normal, Vector3.up));
            if (startHeight.HasValue)
                AddReward(startHeight.Value - movement.Height);
        }

        private void FixationReward(MovementInfo movement)
        {
            float r = Vector3.Dot(movement.Normal, Vector3.up);
            AddReward(r);
        }

        private void TouchedReward(MovementInfo movement)
        {
            FixationReward(movement);
            float r = 3 + 3*CraftManager.StateSettings.MaxTocuhVelocity / (0.01f + movement.Velocity.magnitude);
            AddReward(r);
        }

        private void FatalReward()
        {
            EndEpisode();
        }

        private void LandedReward()
        {
            AddReward(70);
            EndEpisode();
        }
        #endregion
    }
}
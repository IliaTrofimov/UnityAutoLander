using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using URandom = UnityEngine.Random;

using Lander.Thrusters;
using Lander.CraftState;
using Lander.Control;
using Lander.ProximitySensors;
using Unity.MLAgents.Policies;
using GlobalShared;

namespace Lander.AI
{
    /// <summary>Агент ИИ для посадочного модуля.</summary>
    public class AutoLanderAgent : Agent
	{
        [Header("Controls")]
        [SerializeField]
        private ProximitySensorsArray SensorsArray;

        [SerializeField]
        private RootThrustersController ThrustersController;

        [SerializeField]
        private CraftManager CraftManager;

        [SerializeField]
        private Rigidbody Lander;

        [Header("Logging")]
        [SerializeField]
        private bool ShowRewardsLog;

        [SerializeField]
        private bool UseLogging;

        [SerializeField]
        private string RewardLogsFile;

        [SerializeField]
        private string ActionsLogsFile;

        [Header("Training")]
        [SerializeField]
        private AutoLanderAgentTrainingParams TrainingParams;

        [SerializeField]
        private bool ResetOnEpisode;


        private Vector3 initialPos;
        private Vector3 initialRotation;
        private List<ProximitySensor> sensors;
        private float? startHeight;
        private static Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);

        private FlyingReward flyingReward;
        private FixationReward fixationReward;
        private FinalReward fatalReward;
        private FinalReward landedReward;
        private ArrayLogger<float> actionsLogger;


        public void Start()
        {
            initialPos = Lander.worldCenterOfMass;
            initialRotation = Lander.transform.eulerAngles;

            if (TrainingParams != null)
                ResetParams();

            if (UseLogging)
            {
                actionsLogger = new ArrayLogger<float>($"{ActionsLogsFile}/{CraftManager.Name}.csv", 200, new System.TimeSpan(0, 0, 5));
                flyingReward = new FlyingReward(1, 1, 1, RewardLogsFile);
                fixationReward = new FixationReward(1, 1, RewardLogsFile);
                fatalReward = new FinalReward(-1000, RewardLogsFile);
                landedReward = new FinalReward(1000, RewardLogsFile);

            }
            else
            {
                flyingReward = new FlyingReward(1, 1, 1);
                fixationReward = new FixationReward(1, 1);
                fatalReward = new FinalReward(-1000);
                landedReward = new FinalReward(1000);
            }
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
            sensor.AddObservation(state.Movement.Height);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            float up = actionBuffers.ContinuousActions[0] > 0 ? actionBuffers.ContinuousActions[0] : 0;
            float pitch = actionBuffers.ContinuousActions[1];
            float yaw = actionBuffers.ContinuousActions[2];
            float roll = actionBuffers.ContinuousActions[3];

            if (actionsLogger != null)
                actionsLogger.Log(up, pitch, yaw, roll);
            ThrustersController.ApplyMovement(0, up, 0, pitch, yaw, roll);

            if (CraftManager.State is FinalState || ThrustersController.Fuel == 0)
            {
                if (CraftManager.State is LandedState)
                    AddReward(landedReward.GetReward(CraftManager.State.Movement));
                EndEpisode();
            }
            else if (CraftManager.State is FlyingState)
                AddReward(flyingReward.GetReward(CraftManager.State.Movement));
            else if (CraftManager.State is TouchedState)
                AddReward(fixationReward.GetReward(CraftManager.State.Movement));
            else if (CraftManager.State is FixationState)
                AddReward(fixationReward.GetReward(CraftManager.State.Movement));

            if (TrainingParams != null && TrainingParams.ShouldReset(Lander))
            {
                if (ShowRewardsLog)
                    Debug.LogWarning("Out of training bounds");
                EndEpisode();
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxis("Jump");
            continuousActionsOut[1] = Input.GetAxis("Pitch") * 0.3f;
            continuousActionsOut[2] = Input.GetAxis("Yaw") * 0.3f;
            continuousActionsOut[3] = Input.GetAxis("Roll") * 0.3f;
        }

        public override void OnEpisodeBegin()
        {
            startHeight = CraftManager.State.Movement.Height;

            if (TrainingParams != null && ResetOnEpisode)
                ResetParams();

            base.OnEpisodeBegin();
        }


        private void ResetParams()
        {
            Lander.transform.position = initialPos + (URandom.insideUnitSphere - half) * TrainingParams.PositionDispersion;
            Lander.transform.eulerAngles = initialRotation + (URandom.insideUnitSphere - half) * TrainingParams.RotationDispersion;

            Lander.velocity = TrainingParams.InitialVelocity + (URandom.insideUnitSphere - half) * TrainingParams.VelocityDispersion;
            Lander.angularVelocity = TrainingParams.InitialAngularVelocity + (URandom.insideUnitSphere - half) * TrainingParams.AngularVelocityDispersion;

            ThrustersController.ResetFuel();
            CraftManager.Reset();
        }
    }
}
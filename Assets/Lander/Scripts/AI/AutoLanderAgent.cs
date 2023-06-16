using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

using Lander.Thrusters;
using Lander.CraftState;
using Lander.Control;
using Lander.ProximitySensors;
using Lander.Shared;

namespace Lander.AI
{
    [RequireComponent(typeof(CraftManager), typeof(Rigidbody), typeof(BaseThrustersController))]
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
        private bool UseActionLogging;

        [SerializeField]
        private string ActionsLogsFile;

        [SerializeField]
        private bool UseRewardsLogging;

        [SerializeField]
        private string RewardLogsFile;

        [SerializeField]
        private bool DisplayRewards;


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

        private FlyingReward flyingReward = new FlyingReward(1, 1, 0.2f, 1);
        private FixationReward fixationReward = new FixationReward(1, 1, 1);
        private FinalReward fatalReward = new FinalReward(-1000);
        private FinalReward landedReward = new FinalReward(1000);
        private ArrayLogger<float> actionsLogger;
        private Logger<float> rewardsLogger;

        private DateTime episodeStart;
        private bool firstReset;


        public void Start()
        {
            initialPos = Lander.worldCenterOfMass;
            initialRotation = Lander.transform.eulerAngles;

            if (TrainingParams != null)
                firstReset = true;

            if (UseActionLogging)
                actionsLogger = new ArrayLogger<float>($"{ActionsLogsFile}/{CraftManager.Name}.csv", 10);

            if (UseRewardsLogging)
                rewardsLogger = new Logger<float>($"{RewardLogsFile}/{CraftManager.Name}.csv", 10, (f) => $"{f:F3}");

            flyingReward.displayRewards = DisplayRewards;
            fixationReward.displayRewards = DisplayRewards;
            landedReward.displayRewards = DisplayRewards;
            fatalReward.displayRewards = DisplayRewards;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            var state = CraftManager.State;

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
            sensor.AddObservation(SensorsArray.Sensors[0].Distance);
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


            float reward = 0;
            if (CraftManager.State is FinalState || ThrustersController.Fuel == 0)
            {
                if (CraftManager.State is LandedState)
                    AddReward(reward = landedReward.GetReward(CraftManager.State.Movement));

                Debug.Log($"{Lander.name} is in final sate ({CraftManager.State})");
                EndEpisode();
            }
            else if (CraftManager.State is FlyingState)
                AddReward(reward = flyingReward.GetReward(CraftManager.State.Movement));
            else if (CraftManager.State is TouchedState)
                AddReward(reward = fixationReward.GetReward(CraftManager.State.Movement));
            else if (CraftManager.State is FixationState)
                AddReward(reward = fixationReward.GetReward(CraftManager.State.Movement));

            if (TrainingParams != null && TrainingParams.ShouldReset(Lander))
            {
                Debug.Log($"{Lander.name} out of training bounds");
                EndEpisode();
            }
            else if ((DateTime.Now - episodeStart).Seconds > TrainingParams.MaxEpisodeSeconds)
            {
                episodeStart = DateTime.Now;
                Debug.Log($"{Lander.name} too long alive");
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

            if (TrainingParams != null && (ResetOnEpisode || firstReset))
                ResetParams();

            episodeStart = DateTime.Now;
            base.OnEpisodeBegin();
        }


        private void ResetParams()
        {
            firstReset = false;
            Lander.transform.position = MathUtils.RandomVector3(initialPos, TrainingParams.PositionDispersion);
            Lander.transform.eulerAngles = MathUtils.RandomVector3(initialRotation, TrainingParams.RotationDispersion);

            Lander.velocity = MathUtils.RandomVector3(TrainingParams.InitialVelocity, TrainingParams.VelocityDispersion);
            Lander.angularVelocity = MathUtils.RandomVector3(TrainingParams.InitialAngularVelocity, TrainingParams.AngularVelocityDispersion);

            ThrustersController.ResetFuel();
            CraftManager.Reset();
        }
    }
}
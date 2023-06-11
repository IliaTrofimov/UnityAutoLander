using UnityEngine;

using Lander.Shared;
using GlobalShared;
using UnityEditor;

namespace Lander.AI
{
    public class FixationReward : BaseReward
    {
        private readonly float gamma_v = 1;
        private readonly float gamma_n = 1;

        public FixationReward(float gamma_v, float gamma_n)
            : base()
        {
            this.gamma_n = gamma_n;
            this.gamma_v = gamma_v;
        }

        public FixationReward(float gamma_v, float gamma_n, string rewardLogsFile) : base(rewardLogsFile)
        {
            this.gamma_n = gamma_n;
            this.gamma_v = gamma_v;
        }

        protected override float[] GetRewardsVector(MovementInfo movement)
        {
            return new float[]
            {
                gamma_v * Shared.MathUtils.InvSqr(movement.Velocity.magnitude),
                gamma_n * Mathf.Atan(Vector3.Dot(movement.Normal, -movement.Velocity.normalized) * 5 - 4)
            };
        }
    }
}
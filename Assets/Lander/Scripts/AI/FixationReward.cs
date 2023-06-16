using UnityEngine;
using UnityEditor;
using Lander.Shared;

namespace Lander.AI
{
    public class FixationReward : BaseReward
    {
        private readonly float gamma_v = 1;
        private readonly float gamma_n = 1;
        private readonly float gamma_h = 1;


        public FixationReward(float gamma_v, float gamma_n, float gamma_h, bool displayRewards = false)
            : base(displayRewards)
        {
            this.gamma_n = gamma_n;
            this.gamma_v = gamma_v;
            this.gamma_h = gamma_h;
        }

        protected override float[] GetRewardsVector(MovementInfo movement)
        {
            return new float[]
            {
                gamma_v * Shared.MathUtils.InvSqr(movement.Velocity.magnitude, xRoot: 5) - 1,
                gamma_n * Mathf.Atan(Vector3.Dot(movement.Normal, -movement.Velocity.normalized) * 5 - 4),
                gamma_h * Shared.MathUtils.InvSqr(movement.Velocity.magnitude, xRoot: 5) - 1,
            };
        }
    }
}
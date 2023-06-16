using UnityEngine;

using Lander.Shared;
using GlobalShared;

namespace Lander.AI
{
    public class FlyingReward : BaseReward
    {
        private readonly float gamma_v = 1;
        private readonly float gamma_w = 1;
        private readonly float gamma_n = 1;
        private readonly float gamma_h = 1;


        public FlyingReward(float gamma_v, float gamma_n, float gamma_w, float gamma_h, bool displayRewards = false)
            : base(displayRewards)
        {
            this.gamma_n = gamma_n;
            this.gamma_v = gamma_v;
            this.gamma_w = gamma_w;
            this.gamma_h = gamma_h;
        }

        protected override float[] GetRewardsVector(MovementInfo movement)
        {
            return new float[]
            {
                gamma_v * MathUtils.InvSqr(movement.Velocity.magnitude,
                                xRoot: 4 + 3 * Mathf.Log(movement.Height + 1),
                                xOffset: 1 + 3 * Mathf.Log(Mathf.Pow(movement.Height, 2) + 1)),
                gamma_n * Mathf.Atan(Vector3.Dot(movement.Normal, -movement.Velocity.normalized) * 5 - 4),
                gamma_w * (MathUtils.InvSqr(movement.AngularVelocity.magnitude,
                                xRoot: 0.5f) - 1),
                movement.Velocity.y > 0
                    ? gamma_h * Mathf.Atan(-movement.Velocity.y)
                    : 0
            };
        }
    }
}
using Lander.Shared;
using GlobalShared;

namespace Lander.AI
{
    public class FinalReward : BaseReward
    {
        private readonly float reward = 1000;

        public FinalReward(float reward, bool displayRewards = false)
            : base(displayRewards)
        {
            this.reward = reward;
        }


        protected override float[] GetRewardsVector(MovementInfo movement) => new float[] { reward };
    }
}
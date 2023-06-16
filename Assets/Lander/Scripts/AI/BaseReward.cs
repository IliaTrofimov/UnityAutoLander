using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

using Lander.Shared;
using Lander.Control;

namespace Lander.AI
{
    public abstract class BaseReward
    {
        public bool displayRewards;


        public BaseReward() { }
        public BaseReward(bool displayRewards = false) { this.displayRewards = displayRewards; }


        public float GetReward(MovementInfo movement)
        {
            float[] rewards = GetRewardsVector(movement);
            float total = rewards.Sum();

            if (displayRewards)
                StatsDisplay.ShowInfo($"{this.GetType().Name.Replace("Reward", "")}: {string.Join(",", rewards.Select(r => $"{r:F2}"))} = {total:F2}");

            return total;
        }

        public float GetReward(MovementInfo movement, out float[] rewards)
        {
            rewards = GetRewardsVector(movement);
            float total = rewards.Sum();

            if (displayRewards)
                StatsDisplay.ShowInfo($"{this.GetType().Name.Replace("Reward", "")}: {string.Join(",", rewards.Select(r => $"{r:F2}"))} = {total:F2}");

            return total;
        }

        protected abstract float[] GetRewardsVector(MovementInfo movement);
    }
}
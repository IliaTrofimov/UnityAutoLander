using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lander.Shared;
using GlobalShared;
using UnityEngine;

namespace Lander.AI
{
    public abstract class BaseReward
    {
        private static Logger<float[]> logger = null;
        private string rewardLogsFile;

        public BaseReward() { }

        public BaseReward(string rewardLogsFile)
        {
            this.rewardLogsFile = rewardLogsFile;
            if (string.IsNullOrEmpty(rewardLogsFile))
                return;

            logger = new Logger<float[]>(
                rewardLogsFile, 200, new System.TimeSpan(0,0,5), (float[] r) => $"{this.GetType().Name};{string.Join(';', r)}");
        }

        public float GetReward(MovementInfo movement)
        {
            float[] rewards = GetRewardsVector(movement);
            float total = rewards.Sum();
            if (logger != null)
            {
                logger.Log(rewards);
            }
            return total;
        }

        protected abstract float[] GetRewardsVector(MovementInfo movement);
    }
}
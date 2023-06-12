using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

using Lander.Shared;


namespace Lander.AI
{
    public abstract class BaseReward
    {
        private static Logger<float> logger = null;
        private string rewardLogsFile;

        public BaseReward() { }

        public BaseReward(string rewardLogsFile)
        {
            this.rewardLogsFile = rewardLogsFile;
            if (string.IsNullOrEmpty(rewardLogsFile))
                return;

            logger = new Logger<float>(
                rewardLogsFile, 10, (float r) => $"{this.GetType().Name};{r}");
        }

        public float GetReward(MovementInfo movement)
        {
            float[] rewards = GetRewardsVector(movement);
            float total = rewards.Sum();
            if (logger != null)
            {
                Task.Run(() => logger.LogAsync(total));
            }
            
            return total;
        }

        protected abstract float[] GetRewardsVector(MovementInfo movement);
    }
}
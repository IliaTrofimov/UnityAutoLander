using UnityEngine;
using Lander.Shared;

namespace Lander.CraftState
{
    public class LandedState : FinalState
    {
        public LandedState(MovementInfo movement, StateSettings settings, bool isStateChanged = true)
            : base(movement, settings, isStateChanged)
        {
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, settings, true);
            else
                return base.NextState(newMovement);
        }
    }

}
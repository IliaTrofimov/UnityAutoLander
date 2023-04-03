using UnityEngine;
using Shared;

namespace CraftState
{
    public class LandedState : BaseState
    {
        public LandedState(MovementInfo movement, bool isStateChanged = false) : base(movement, isStateChanged) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, true);
            else
                return base.NextState(newMovement);
        }
    }

}
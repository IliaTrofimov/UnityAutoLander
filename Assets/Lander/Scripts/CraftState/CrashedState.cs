﻿using Shared;

namespace CraftState
{
    public class CrashedState : BaseState
    {
        public CrashedState(MovementInfo movement, bool isStateChanged = true) : base(movement, isStateChanged)
        {
            isFinalState = true;
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, true);
            else
                return base.NextState(newMovement);
        }
    }
}
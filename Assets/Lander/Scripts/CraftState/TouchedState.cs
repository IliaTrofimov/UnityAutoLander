using UnityEngine;
using Shared;

namespace CraftState
{
    public class TouchedState : BaseState
    {
        public TouchedState(MovementInfo movement, bool isStateChanged = true) : base(movement, isStateChanged) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, true);
            else
                return new FixationState(newMovement, movement.Position, movement.Normal, createdAt, true);
        }
    }
}
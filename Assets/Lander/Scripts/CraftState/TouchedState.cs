using UnityEngine;
using Lander.Shared;

namespace Lander.CraftState
{
    public class TouchedState : BaseState
    {
        public TouchedState(MovementInfo movement, StateSettings settings, bool isStateChanged = true)
            : base(movement, settings, isStateChanged) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided && newMovement.Height - movement.Height > 1)
                return new FlyingState(newMovement, settings, true);
            else
                return new FixationState(newMovement, movement.Position, movement.Normal, createdAt, settings, true);
        }
    }
}
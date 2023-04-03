using UnityEngine;
using Shared;

namespace CraftState
{
    public class FlyingState : BaseState
	{
        public FlyingState(MovementInfo movement, bool isStateChanged = true) : base(movement, isStateChanged) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (newMovement.IsCollided)
                return newMovement.Velocity.magnitude > MaxTocuhVelocity ? new CrashedState(newMovement) : new TouchedState(movement);
            else if (Vector3.Dot(newMovement.Normal, Vector3.up) < -0.35f)
                return new OverturnedState(newMovement);             
            else
                return base.NextState(newMovement);
        }
	}
}
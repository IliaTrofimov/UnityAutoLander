using UnityEngine;
using Lander.Shared;

namespace Lander.CraftState
{
    public class FlyingState : BaseState
	{
        public FlyingState(MovementInfo movement, StateSettings settings = default, bool isStateChanged = true)
            : base(movement, settings, isStateChanged) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (newMovement.IsCollided)
                return newMovement.Velocity.magnitude > settings.MaxTocuhVelocity
                    ? new CrashedState(newMovement, settings)
                    : new TouchedState(newMovement, settings);
            else if (Vector3.Dot(newMovement.Normal, Vector3.up) < -0.35f)
                return new OverturnedState(newMovement, settings);             
            else
                return base.NextState(newMovement);
        }
	}
}
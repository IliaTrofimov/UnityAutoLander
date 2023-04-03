using Shared;

namespace CraftState
{
    public class FlyingState : BaseState
	{
        public static float MaxTocuhVelocity = 40;

        public FlyingState(MovementInfo movement, bool isStateChanged = false) : base(movement, isStateChanged) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (newMovement.IsCollided)
                return newMovement.Velocity.magnitude > MaxTocuhVelocity ? new CrashedState(newMovement, true) : new TouchedState(movement, true);
            else
                return base.NextState(newMovement);
        }
	}
}
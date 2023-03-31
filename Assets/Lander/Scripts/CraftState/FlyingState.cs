namespace CraftState
{
    public class FlyingState : BaseState
	{
        public FlyingState(MovementInfo movement) : base(movement) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (newMovement.IsCollided)
                return newMovement.Velocity.magnitude > 20 ? new CrashedState(newMovement) : new TouchedState(movement);
            else
                return base.NextState(newMovement);
        }
	}
}
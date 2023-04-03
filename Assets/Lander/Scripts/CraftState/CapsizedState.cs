using Shared;

namespace CraftState
{
    public class CapsizedState : BaseState
    {
        public CapsizedState(MovementInfo movement, bool isStateChanged = false) : base(movement, isStateChanged) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, true);
            else
                return base.NextState(newMovement);
        }
    }

    public class SlippedState : CapsizedState
    {
        public SlippedState(MovementInfo movement, bool isStateChanged = false) : base(movement, isStateChanged) { }
    }
}
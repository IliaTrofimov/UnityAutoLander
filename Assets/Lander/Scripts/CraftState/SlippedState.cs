using Shared;

namespace CraftState
{
    public class SlippedState : CapsizedState
    {
        public SlippedState(MovementInfo movement, bool isStateChanged = true) : base(movement, isStateChanged)
        {
            isFinalState = true;
        }
    }
}
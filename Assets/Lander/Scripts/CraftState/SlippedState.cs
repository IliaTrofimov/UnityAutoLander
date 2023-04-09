using Lander.Shared;

namespace Lander.CraftState
{
    public class SlippedState : CapsizedState
    {
        public SlippedState(MovementInfo movement, StateSettings settings, bool isStateChanged = true)
            : base(movement, settings, isStateChanged)
        {
        }
    }
}
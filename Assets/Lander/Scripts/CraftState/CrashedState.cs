using Lander.Shared;

namespace Lander.CraftState
{
    public class CrashedState : FatalState
    {
        public CrashedState(MovementInfo movement, StateSettings settings, bool isStateChanged = true)
            : base(movement, settings, isStateChanged)
        {
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, settings, true);
            else
                return base.NextState(newMovement);
        }
    }
}
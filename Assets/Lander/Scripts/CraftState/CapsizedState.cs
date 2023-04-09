using Lander.Shared;

namespace Lander.CraftState
{
    public class CapsizedState : FatalState
    {
        public CapsizedState(MovementInfo movement, StateSettings settings = default, bool isStateChanged = true)
            : base(movement, settings, isStateChanged)
        {
            this.settings = settings;
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
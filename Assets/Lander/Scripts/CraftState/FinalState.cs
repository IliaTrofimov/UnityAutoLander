using Lander.Shared;

namespace Lander.CraftState
{
    public abstract class FinalState : BaseState
    {
        public FinalState(MovementInfo movement, StateSettings settings, bool isStateChanged = true) :
            base(movement, settings, isStateChanged)
        { }

        public override string ToString()
        {
            return base.ToString() + "*";
        }
    }
}
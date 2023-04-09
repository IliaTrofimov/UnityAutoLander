using Lander.Shared;

namespace Lander.CraftState
{
    public abstract class FatalState : FinalState
    {
        public FatalState(MovementInfo movement, StateSettings settings = default, bool isStateChanged = true) :
            base(movement, settings, isStateChanged)
        { }

        public override string ToString()
        {
            return base.ToString() + "!";
        }
    }
}
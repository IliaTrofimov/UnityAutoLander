using UnityEngine;

namespace CraftState
{
    public class LandedState : BaseState
    {
        public LandedState(MovementInfo movement) : base(movement) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement);
            else if (Vector3.Distance(movement.Position, newMovement.Position) > 5 || Vector3.Distance(movement.EulerAngles, newMovement.EulerAngles) > 20)
                return new CapsizedState(newMovement);
            else
                return base.NextState(newMovement);
        }
    }

}
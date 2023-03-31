using UnityEngine;

namespace CraftState
{
    public class TouchedState : BaseState
    {
        public TouchedState(MovementInfo movement) : base(movement) { }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement);
            else if (Vector3.Distance(movement.Position, newMovement.Position) > 5 || Vector3.Distance(movement.EulerAngles, newMovement.EulerAngles) > 5)
                return new CapsizedState(newMovement);
            else
                return new FixationState(newMovement, movement.Position, movement.EulerAngles, createdAt);
        }
    }

}
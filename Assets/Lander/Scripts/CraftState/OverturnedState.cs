using UnityEngine;
using Shared;

namespace CraftState
{
    public class OverturnedState : BaseState
    {
        public OverturnedState(MovementInfo movement, bool isStateChanged = true) : base(movement, isStateChanged)
        {
            isFinalState = true;
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (newMovement.IsCollided)
                return new CrashedState(newMovement);
            else if (Vector3.Dot(newMovement.Normal, Vector3.up) >= -0.35f)
                return new FlyingState(newMovement);
            else
                return base.NextState(newMovement);
        }
    }
}